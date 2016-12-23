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
        bool SetScore(Match match, Credentials credentials);
        IList<Match> GetAllMatches();
        IList<Match> GetAllMatchesFor(Player player);
        IList<Match> GetAllMatchesFor(Tournament tournament);
        IList<Match> GetAllUnfinishedMatches();
    }
}
