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
    [RoutePrefix("api/rating")]
    public class RatingController : BaseController
    {
        private IRatingManager Logic { get; } = BLFactory.GetRatingManager();

        [HttpGet]
        [Route("player/{playerId}", Name = "GetCurrentRatingByPlayerIdRoute")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Player not found")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns rating for player with that id", typeof(Rating))]
        public Rating GetCurrentByPlayerId(int playerId)
        {
            var rating = Logic.GetCurrentRatingFor(playerId);

            if (rating == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return rating;
        }
    }
}
