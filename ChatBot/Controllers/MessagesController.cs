using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DefaceWebsiteService;
using ChatBot.Models;
using Microsoft.EntityFrameworkCore;
using ChatBot.Infrastructure.Core;
using AutoMapper;

namespace ChatBot.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        int _page = 1;
        int _pageSize = 10;
        public async Task<IEnumerable<Messages>> Get()
        {
            var pagination = Request.Headers["Pagination"];
            
            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out _page);
                int.TryParse(vals[1], out _pageSize);
            }
            
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await context.Messages.FromSql("dbo.Messages_Search @USER = {0}, @DOMAIN = {1} , @STATUS = {2}","thieu1234", null,null).ToArrayAsync();

            int currentPage = _page;
            int currentPageSize = _pageSize;

            var totalRecord = result.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

            var messages = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            IEnumerable<Messages> listPagedMessage = Mapper.Map<IEnumerable<Messages>, IEnumerable<Messages>>(messages);
            return listPagedMessage;
            // MessagesClient client = new MessagesClient();
            // return await client.Messages_SearchAsync("thieu1234", "", "");

        }

    }
}