using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Optimization;
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

        public Task MatchSave(Match match)
        {
            if (match?.MatchId == null || match.ScoreTeam1 == null || match.ScoreTeam2 == null) return Task.FromResult(0);

            var localMatch = _matches.First(m => m.MatchId == match.MatchId);
            localMatch.ScoreTeam1 = match.ScoreTeam1;
            localMatch.ScoreTeam2 = match.ScoreTeam2;
            BroadcastMatches();
            _matchManager.SetScore(match);
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
            if (_matches == null || _matches.Count == 0)
            {
                return base.OnConnected();
            }
            Clients.Caller.broadcastMatches(_matches);
            return base.OnConnected();
        }
    }
}