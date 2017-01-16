using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface IPlayerManager
    {
        bool AddPlayer(Player player);
        bool UpdatePlayer(Player player);
        bool ChangePassword(string username, string newPassword);
        Player GetPlayer(int playerId);
        Player GetPlayer(string username);
        IList<Player> GetAllPlayers();
    }
}
