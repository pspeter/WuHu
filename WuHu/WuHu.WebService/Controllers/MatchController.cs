using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using WuHu.BL;
using WuHu.BL.Impl;
using WuHu.Domain;

namespace WuHu.WebService.Controllers
{
    [RoutePrefix("api/match")]
    public class MatchController : ApiController
    {

        private IMatchManager Logic { get; } = BLFactory.GetMatchManager();
    }
}
