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
namespace ChatBot.Controllers
{

   // [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        
        public async Task<IEnumerable<Users>> Get()
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            //var rs = await context.Database.ExecuteSqlCommandAsync("Users_Search @p0", cancellationToken: CancellationToken.None,parameters: new[] {"thieu1234" });
            //int x = 1;
            var result = await context.Users.FromSql("Users_Search @p0", "thieu1234").ToListAsync();
            int a = 5;
            return result;
            //UserClient client = new UserClient();
            //client.Endpoint.Binding.SendTimeout=new TimeSpan(0, 30, 0);
            // client.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 30, 0);

            //return await client.Users_ByParentAsync(null, "thieu1234");

        }
        [HttpDelete("{id:int}")]
        public async Task<Users_DelResult> Delete(int  ID)
        {
            UserClient client = new UserClient();
            Users_DelResult result = await client.Users_DelAsync(ID);
            return result;
        }
    }
}