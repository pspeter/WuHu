using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using WuHu.Domain;
using WuHu.BL;
using WuHu.BL.Impl;

namespace WuHu.WebService.Controllers
{
    [RoutePrefix("api/player")]
    public class PlayerController : BaseController
    {
        private IPlayerManager Logic { get; } = BLFactory.GetPlayerManager();

        [HttpGet]
        [Route("{playerId}", Name = "GetPlayerByIdRoute")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Player not found")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns player with that id", typeof(Player))]
        public Player GetById(int playerId)
        {
            var player = Logic.GetPlayer(playerId);

            if (player == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return player;
        }

        [HttpGet]
        [Route("{username}", Name = "GetPlayerByUsernameRoute")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Player not found")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns player with that username", Type = typeof(Player))]
        public Player GetByUsername(string username)
        {
            var player = Logic.GetPlayer(username);

            if (player == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return player;
        }

        [Authorize]
        [HttpGet]
        [Route("", Name = "GetAllPlayersRoute")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns all players", Type = typeof(IEnumerable<Player>))]
        public IEnumerable<Player> GetAll()
        {
            var players = Logic.GetAllPlayers();
            return players;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("", Name = "PostPlayerRoute")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public void PostPlayer([FromBody] Player player)
        {
            var success = Logic.AddPlayer(player, null);

            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }
    }
}
