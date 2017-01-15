using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Dal.Common
{
    public interface IMatchDao
    {
        IList<Match> FindAll();
        IList<Match> FindAllByPlayer(Player player);
        IList<Match> FindAllByTournament(Tournament tournament);
        Match FindCurrentByPlayer(int playerId);
        Match FindById(int matchId);
        bool Insert(Match match);
        bool Update(Match match);
        bool Delete(Match match);
        int Count();
    }
}
