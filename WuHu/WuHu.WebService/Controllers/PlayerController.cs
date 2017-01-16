using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
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
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public void PostPlayer([FromBody] Player player)
        {

            var success = Logic.AddPlayer(player);

            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("", Name = "PutPlayerRoute")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.NotFound, "Player not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public void PutPlayer([FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var fullPlayer = Logic.GetPlayer(player.Username);

            if (fullPlayer == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // complete player
            player.Salt = fullPlayer.Salt;
            player.Password = fullPlayer.Password;

            var success = Logic.UpdatePlayer(player);

            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
