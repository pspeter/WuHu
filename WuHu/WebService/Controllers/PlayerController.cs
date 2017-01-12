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

namespace WebService.Controllers
{
    public class PlayerController : ApiController
    {
        private IPlayerManager Logic { get; } = BLFactory.GetPlayerManager();

        [HttpGet]
        [Route("player/{playerId}", Name = "GetByIdRoute")]
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
        [Route("player/{username}", Name = "GetByUsernameRoute")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public Player GetByUsername(string user)
        {
            var player = Logic.GetPlayer(user);

            if (player == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return player;
        }

        [HttpGet]
        [Route("player", Name = "GetAllRoute")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IList<Player> GetAll()
        {
            var players = Logic.GetAllPlayers();
            return players;
        }
    }
}
