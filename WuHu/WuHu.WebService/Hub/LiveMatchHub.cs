using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Optimization;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using WuHu.BL;
using WuHu.BL.Impl;
using WuHu.Domain;

namespace WuHu.WebService.Hub
{
    public class LiveMatchHub : Microsoft.AspNet.SignalR.Hub
    {
        private static IList<Match> _matches = new List<Match>();
        private static Tournament _tournament;
        private readonly IMatchManager _matchManager = BLFactory.GetMatchManager();
        private readonly ITournamentManager _tournamentManager = BLFactory.GetTournamentManager();

        public Task MatchSave(int matchId, byte score1, byte score2)
        {
            if (matchId < 0) return Task.FromResult(0);

            var localMatch = _matches.First(m => m.MatchId == matchId);
            localMatch.ScoreTeam1 = score1;
            localMatch.ScoreTeam2 = score2;
            BroadcastAll();
            _matchManager.SetScore(localMatch);
            return Task.FromResult(0);
        }

        public async Task RefreshMatchesAsync()
        {
            _matches = await Task.Run(() => _matchManager.GetAllCurrentMatches());
            _tournament = await Task.Run(() => _tournamentManager.GetMostRecentTournament());
            Clients.All.broadcastMatches(_matches);
            Clients.All.broadcastTournamentName(_tournament?.Name);
        }

        public Task RefreshMatches()
        {
            return RefreshMatchesAsync();
        }

        private void BroadcastAll()
        {
            BroadcastMatches();
            BroadcastTournamentName();
        }

        private void BroadcastMatches()
        {
            Clients.Others.broadcastMatches(_matches);
        }

        private void BroadcastTournamentName()
        {
            Clients.Others.broadcastTournamentName(_tournament?.Name);
        }

        public override async Task OnConnected()
        {
            if (_matches == null || _matches.Count == 0)
            {
                await RefreshMatchesAsync();
            }
            Clients.Caller.broadcastMatches(_matches);
            Clients.Caller.broadcastTournamentName(_tournament.Name);
            await base.OnConnected();
        }

        public override Task OnReconnected()
        {
            Clients.Caller.broadcastMatches(_matches);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}