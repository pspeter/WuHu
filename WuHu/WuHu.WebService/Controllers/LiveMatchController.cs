using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WuHu.WebService.Hub;

namespace WuHu.WebService.Controllers
{


    [RoutePrefix("api/live")]
    public class LiveMatchController : ApiController
    {
        private IHubContext _context;

        public LiveMatchController()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<LiveMatchHub>();
        }

        [HttpGet]
        [Route("matches")]
        public IHttpActionResult GetLiveMatch()
        {
            _context.Clients.All.Send("hey");
            return Ok();
        }

        [HttpPost]
        [Route("matches")]
        public IHttpActionResult PostLiveMatch([FromBody] string message)
        {
            _context.Clients.All.Send(message);
            return Ok();
        }
    }
}
