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
        IList<Player> FindAllOnDays(bool monday = false, bool tuesday = false, bool wednesday = false, 
            bool thursday = false, bool friday = false, bool saturday = false, bool sunday = false);
        IList<Player> FindAllByString(string name);
        Player FindById(int playerId);
        Player FindByUsername(string username);
        int Insert(Player player);
        bool Update(Player player);
        int Count();
    }
}
