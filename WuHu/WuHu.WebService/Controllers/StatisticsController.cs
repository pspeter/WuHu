using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using WuHu.BL;
using WuHu.BL.Impl;
using WuHu.WebService.Models;

namespace WuHu.WebService.Controllers
{
    [RoutePrefix("api/stats")]
    public class StatisticsController : ApiController
    {
        private IPlayerManager PlayerLogic { get; } = BLFactory.GetPlayerManager();
        private IMatchManager MatchLogic { get; } = BLFactory.GetMatchManager();

        [HttpGet]
        [Route("player/username/{username}", Name = "GetStatsByPlayerId")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK, "Returns statistical data about the player with that id", typeof(PlayerStats))]
        public PlayerStats GetPlayerStats(string username)
        {
            if (username == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var player = PlayerLogic.GetPlayer(username);

            if (player == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            
            return new PlayerStats(player);
        }

        [HttpGet]
        [Route("ranklist", Name = "GetRanklistData")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK, "Returns data for the ranklist", typeof(IEnumerable<RanklistData>))]
        public IEnumerable<RanklistData> GetRanklistData()
        {
            var players = PlayerLogic.GetAllPlayers();

            return players.Select(player => new RanklistData(player));
        }
    }
}
