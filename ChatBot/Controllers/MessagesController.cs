using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DefaceWebsiteService;
namespace ChatBot.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        public async Task<IEnumerable<Messages_SearchResult>> Get()
        {
            
            MessagesClient client = new MessagesClient();
            return await client.Messages_SearchAsync("thieu1234", "", "");
            
        }

    }
}