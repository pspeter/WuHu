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
        private readonly IMatchManager _matchManager = BLFactory.GetMatchManager();



        public void Save(string message)
        {
            Send(message + "test");
        }

        public void Send(string message)
        {
            Clients.All.send(message);
        }

        public Task MatchSave(int matchId, byte score1, byte score2)
        {
            if (matchId < 0) return Task.FromResult(0);

            var localMatch = _matches.First(m => m.MatchId == matchId);
            localMatch.ScoreTeam1 = score1;
            localMatch.ScoreTeam2 = score2;
            BroadcastMatches();
            _matchManager.SetScore(localMatch);
            return Task.FromResult(0);
        }

        public void BroadcastMatches()
        {
            Clients.Others.broadcastMatches(_matches);
        }

        public override Task OnConnected()
        {
            if (_matches == null || _matches.Count == 0)
            {
                _matches = _matchManager.GetAllUnfinishedMatches();
            }
            Clients.Caller.broadcastMatches(_matches);
            return base.OnConnected();
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