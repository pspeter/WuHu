using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Dal.Common
{
    public interface IPlayerDao
    {
        IList<Player> FindAll();
        IList<Player> FindAllOnDays(bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday);
        Player FindById(int playerId);
        int Insert(Player player);
        bool Update(Player player);
    }
}
