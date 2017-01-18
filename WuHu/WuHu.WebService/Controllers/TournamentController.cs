﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using WuHu.BL;
using WuHu.BL.Impl;
using WuHu.Domain;
using WuHu.WebService.Models;

namespace WuHu.WebService.Controllers
{
    [RoutePrefix("api/tournament")]
    public class TournamentController : ApiController
    {
        private ITournamentManager Logic { get; } = BLFactory.GetTournamentManager();

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("current", Name = "GetCurrentTournament")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns current tournament", typeof(TournamentData))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No tournament found")]
        public TournamentData GetCurrentTournament()
        {
            var tournament = Logic.GetMostRecentTournament();

            if (tournament?.TournamentId == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new TournamentData(tournament);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("lock/{tournamentId}", Name = "LockTournamentRoute")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public void LockTournament([FromBody] TournamentData tournament)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var success = Logic.LockTournament(new Tournament(tournament.Name, tournament.Datetime));

            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("unlock/{tournamentId}", Name = "UnlockTournamentRoute")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public void UnlockTournament([FromBody] TournamentData tournament)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Logic.UnlockTournament(new Tournament(tournament.Name, tournament.Datetime));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("", Name = "PostTournamentRoute")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public void PostTournament([FromBody] TournamentData tournament)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var success = Logic.CreateTournament(
                new Tournament(tournament.Name, tournament.Datetime),
                tournament.Players, tournament.Amount);

            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("", Name = "PutTournamentRoute")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public void PutTournament([FromBody] TournamentData tournament)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var success = Logic.LockTournament(
                new Tournament(tournament.TournamentId, tournament.Name, tournament.Datetime));

            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }
            
            success = Logic.UpdateTournament(
                new Tournament(tournament.TournamentId, tournament.Name, tournament.Datetime),
                tournament.Players, tournament.Amount);

            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
