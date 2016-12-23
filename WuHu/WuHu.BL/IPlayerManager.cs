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
        bool AddPlayer(Player player, Credentials credentials);
        bool UpdatePlayer(Player player, Credentials credentials);
        bool ChangePassword(string username, string newPassword, Credentials credentials);
        Player GetPlayer(int playerId);
        Player GetPlayer(string username);
        IList<Player> GetAllPlayers();
        IList<Player> GetRanklist();
    }
}
