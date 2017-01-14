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
    public class PlayerController : ApiController
    {
        private IPlayerManager Logic { get; } = BLFactory.GetPlayerManager();

        [HttpGet]
        [Route("{playerId}", Name = "GetByIdRoute")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK)]
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
        [Route("{username}", Name = "GetByUsernameRoute")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public Player GetByUsername(string username)
        {
            var player = Logic.GetPlayer(username);

            if (player == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return player;
        }

        [HttpGet]
        [Route("", Name = "GetAllRoute")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IList<Player> GetAll()
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
