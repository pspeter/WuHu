using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface IMatchManager
    {
        bool SetScore(Match match);
        bool SetFinalScore(Match match);
        IList<Match> GetAllMatches();
        IList<Match> GetAllMatchesFor(Player player);
        IList<Match> GetAllMatchesFor(Tournament tournament);
        IList<Match> GetAllCurrentMatches();
        IList<Match> GetAllUnfinishedMatches();
        IList<Match> GetUnfinishedMatchesFor(string username);
    }
}
