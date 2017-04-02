using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DefaceWebsiteService;
using ChatBot.Models;
using Microsoft.EntityFrameworkCore;
using ChatBot.Infrastructure.Core;
using AutoMapper;
using Localization.SqlLocalizer.DbStringLocalizer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChatBot.Controllers
{


    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        int _page = 1;
        int _pageSize = 10;
        private IStringExtendedLocalizerFactory _stringExtendedLocalizerFactory;

        private IAuthorizationService _authorizationService;

        public MessagesController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        //  [Authorize(Policy = "AdminOnly")]
        [HttpGet("{page:int=0}/{pageSize=12}")]
        public async Task<IActionResult> Get(int? page, int? pageSize)
        {
            //var file = Request.;
            PaginationSet<Messages> pagedSet = new PaginationSet<Messages>();
         //   var pagination = Request.Headers;

            //if (!string.IsNullOrEmpty(pagination))
            //{
            //    string[] vals = pagination.ToString().Split(',');
            //    int.TryParse(vals[0], out _page);
            //    int.TryParse(vals[1], out _pageSize);
            //}
            //if (await _authorizationService.AuthorizeAsync(User, "AdminOnly"))
            //{
                DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
                var result =
                    await context.Messages.FromSql("dbo.Messages_Search @USER = {0}, @DOMAIN = {1} , @STATUS = {2}",
                        "thieu1234", null, null).ToArrayAsync();

                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;

                var totalRecord = result.Count();
                var totalPages = (int) Math.Ceiling((double) totalRecord / _pageSize);

                var messages = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
                Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
                IEnumerable<Messages> listPagedMessage =
                    Mapper.Map<IEnumerable<Messages>, IEnumerable<Messages>>(messages);

                pagedSet = new PaginationSet<Messages>()
                {
                    Page = currentPage,
                    TotalCount = totalRecord,
                    TotalPages = totalPages,
                    Items = listPagedMessage
                };
                return new ObjectResult(pagedSet);
         //   }
            //CodeResultStatus _codeResult = new CodeResultStatus(401);
            //return new ObjectResult(_codeResult);
        }

        //public async Task<IEnumerable<Messages>> Get()
        //{
        //    var pagination = Request.Headers["Pagination"];

        //    if (!string.IsNullOrEmpty(pagination))
        //    {
        //        string[] vals = pagination.ToString().Split(',');
        //        int.TryParse(vals[0], out _page);
        //        int.TryParse(vals[1], out _pageSize);
        //    }

        //    DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
        //    var result = await context.Messages.FromSql("dbo.Messages_Search @USER = {0}, @DOMAIN = {1} , @STATUS = {2}","thieu1234", null,null).ToArrayAsync();

        //    int currentPage = _page;
        //    int currentPageSize = _pageSize;

        //    var totalRecord = result.Count();
        //    var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

        //    var messages = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
        //    Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
        //    IEnumerable<Messages> listPagedMessage = Mapper.Map<IEnumerable<Messages>, IEnumerable<Messages>>(messages);
        //    return listPagedMessage;
        //    // MessagesClient client = new MessagesClient();
        //    // return await client.Messages_SearchAsync("thieu1234", "", "");

        //}
        [HttpPost]
        public IActionResult UploadFile(string item)
        {
          //  _stringExtendedLocalizerFactory = new SqlStringLocalizerFactory(this);

        //    var a = _stringExtendedLocalizerFactory.GetLocalizationData();

            var request = this.Request;
            //var files = Request.Files;

            //   var x = request.Body;
            long size = 0;
        //    var host = $"{context.Request.Scheme}";
         //   int a = 5;
            //foreach (var file in files)
            //{
            //    var filename = ContentDispositionHeaderValue
            //                    .Parse(file.ContentDisposition)
            //                    .FileName
            //                    .Trim('"');
            //    filename = hostingEnv.WebRootPath + $@"\{fileName}";
            //    size += file.Length;
            //    using (FileStream fs = System.IO.File.Create(filename))
            //    {
            //        file.CopyTo(fs);
            //        fs.Flush();
            //    }
            //}
            //ViewBag.Message = $"{files.Count} file(s) / 
            //          { size}
            //bytes uploaded successfully!";
            return View();

            //  var file = HttpContext.Current.Request.Files[0];


            //  if (file.ContentLength > 0)
            //  {
            //      var fileName = Path.GetFileName(file.FileName);
            ////      var path = Path.Combine(Microsoft.SqlServer.Server.MapPath("~/App_Data/uploads"), fileName);
            //      file.SaveAs(path);
            //      var content = JsonConvert.SerializeObject(serverFileName, new JsonSerializerSettings
            //      {
            //          ContractResolver = new CamelCasePropertyNamesContractResolver()
            //      });

            //   //   var response = Request.CreateResponse(HttpStatusCode.OK);
            //      response.Content = new StringContent(content, Encoding.UTF8, "application/json");
            //      return response;
            //  }




        }
    }
}