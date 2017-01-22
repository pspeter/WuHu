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
    public class RatingController : ApiController
    {
        private IRatingManager Logic { get; } = BLFactory.GetRatingManager();

        [HttpGet]
        [Route("page/{page}", Name = "GetRatings")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns rating for player with that id", typeof(IEnumerable<Rating>))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IEnumerable<Rating> GetAllRatings(int page)
        {
            if (page < 0)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var ratings = Logic.GetAllRatings(page);
            return ratings;
        }

        [HttpGet]
        [Route("page/count")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns amount of pages for ratings", typeof(int))]
        public int GetPageCount()
        {
            return Logic.GetPageCount();
        }

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
