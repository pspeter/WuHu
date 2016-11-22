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
        Match FindById(int matchId);
        int Insert(Match match);
        bool Update(Match match);
        int Count();
    }
}
