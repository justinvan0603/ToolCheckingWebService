using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DefaceWebsiteService;
using Microsoft.AspNetCore.Cors;
using ChatBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Data.SqlClient;
using ChatBot.Infrastructure.Core;
using ChatBot.Infrastructure.MD5Encryption;
using System.Xml.Linq;
using AutoMapper;

namespace ChatBot.Controllers
{

    // [EnableCors("CorsPolicy")]
    // [Produces("application/json")]
    //[Route("api/Users")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        int _page = 1;
        int _pageSize = 10;
        [HttpGet]
        public async Task<IEnumerable<Users>> Get()
        {
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out _page);
                int.TryParse(vals[1], out _pageSize);
            }

            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await context.Users.FromSql("dbo.Users_ByParent @p_PARENT_ID = {0}, @p_PARENT_NAME = {1}", 1056, "thieu1234").ToArrayAsync();
            int currentPage = _page;
            int currentPageSize = _pageSize;

            var totalRecord = result.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

            var users = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            IEnumerable<Users> listPagedUser = Mapper.Map<IEnumerable<Users>, IEnumerable<Users>>(users);
            

            return listPagedUser;


        }
        /*
         * @p_USERNAME	varchar(50)  = NULL,
            @p_FULLNAME	nvarchar(200)  = NULL,
            @p_PASSWORD	varchar(50)  = NULL,
            @p_EMAIL	varchar(50)  = NULL,
            @p_PHONE	int = NULL,
            @p_PARENT_ID	int = NULL,
            @p_DESCRIPTION	nvarchar(500)  = NULL,
            @p_RECORD_STATUS	varchar(1)  = NULL,
            @p_AUTH_STATUS	varchar(1)  = NULL,
            @p_CREATE_DT	VARCHAR(20) = NULL,
            @p_APPROVE_DT	VARCHAR(20) = NULL,
            @p_EDIT_DT	VARCHAR(20) = NULL,
            @p_MAKER_ID	varchar(15)  = NULL,
            @p_CHECKER_ID	varchar(15)  = NULL,
            @p_EDITOR_ID	varchar(15)  = NULL,
            @DOMAIN XML = NULL
         */
         
        [HttpPost]
        public async Task<int> Post([FromBody]UserObject users)
        {

            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            string pass = MD5Encoder.MD5Hash(users.Password);
            XElement xmldata = new XElement(new XElement("Root"));
            XElement x = new XElement("Domain", new XElement("DOMAIN", users.Domain),
                                               new XElement("DESCRIPTION", users.DomainDesc));
            xmldata.Add(x);

            string command = $"dbo.Users_Ins @p_USERNAME = '{users.Username}', @p_FULLNAME='{users.Fullname}',@p_PASSWORD = '{pass}',@p_EMAIL = '{users.Email}',@p_PHONE = {users.Phone},@p_PARENT_ID = {users.ParentId},@p_DESCRIPTION = '{users.Description}',@p_RECORD_STATUS = '{users.RecordStatus}',@p_AUTH_STATUS = '{users.AuthStatus}',@p_CREATE_DT = '{DateTime.Now}',@p_APPROVE_DT ='',@p_EDIT_DT='',@p_MAKER_ID ='{users.MakerId}',@p_CHECKER_ID = '{users.CheckerId}',@p_EDITOR_ID = '{users.EditorId}',@DOMAIN ={xmldata}";
            var result = await context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
            return result;
            //var i = 1;
            ///return 1;
        }
        /*@p_ID	int = NULL,
@p_USERNAME	varchar(50)  = NULL,
@p_FULLNAME	nvarchar(200)  = NULL,
@p_PASSWORD	varchar(50)  = NULL,
@p_EMAIL	varchar(50)  = NULL,
@p_PHONE	int = NULL,
@p_PARENT_ID	int = NULL,
@p_DESCRIPTION	nvarchar(500)  = NULL,
@p_RECORD_STATUS	varchar(1)  = NULL,
@p_AUTH_STATUS	varchar(1)  = NULL,
@p_CREATE_DT	VARCHAR(20) = NULL,
@p_APPROVE_DT	VARCHAR(20) = NULL,
@p_EDIT_DT	VARCHAR(20) = NULL,
@p_MAKER_ID	varchar(15)  = NULL,
@p_CHECKER_ID	varchar(15)  = NULL,
@p_EDITOR_ID	varchar(15)  = NULL,
@DOMAIN XML = NULL,*/
        [HttpPut]
        public async Task<int> Put(Users user)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            string password = MD5Encoder.MD5Hash(user.Password);
            string command = $"dbo.Users_Upd @p_ID={user.Id},@p_USERNAME = {user.Username},@p_FULLNAME={user.Fullname},@p_PASSWORD={password}";
            var result = await context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
            return result;
        }
        [HttpGet("{id:int}", Name = "GetUser")]
        public async Task<IEnumerable<Users>> Get(int id)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await context.Users.FromSql("dbo.Users_ByParent @p_USERNAME = {0}, @p_USERID = {1}", "thieu1234", 1056).ToArrayAsync();
            return result;
        }
        //[Route("Delete")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await context.Database.ExecuteSqlCommandAsync("Users_Del @ID = {0}", cancellationToken: CancellationToken.None,parameters: id);
            //return result;
            GenericResult removeResult = new GenericResult();
            if (result == 1)
            {
                removeResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Users removed."
                };
            }
            else
            {
                removeResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = "Failed to delete"
                };
            }
            ObjectResult deleteResult = new ObjectResult(removeResult);
            return deleteResult;
        }
    }
}