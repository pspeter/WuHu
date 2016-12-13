using System.Collections.Generic;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface IPlayerManager
    {
        bool AddPlayer(Player player, Credentials credentials);
        bool UpdatePlayer(Player player, Credentials credentials);
        Player GetPlayer(int playerId);
        Player GetPlayer(string username);
        IList<Player> GetAllPlayers();
    }
}
