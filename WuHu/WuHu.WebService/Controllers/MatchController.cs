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

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        [Route("player/{username}", Name = "GetUnfinishedForPlayerRoute")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Match or Player not found")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns matches for that player", typeof(IList<Match>))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IList<Match> GetUnfinishedForPlayer(string username)
        {
            if (username == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var matches = Logic.GetUnfinishedMatchesFor(username);

            if (matches == null || matches.Count == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return matches;
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        [Route("", Name = "PostIntermediateResult")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public void PostIntermediateResult([FromBody] Match match)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var success = Logic.SetScore(match);

            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

    }
}
