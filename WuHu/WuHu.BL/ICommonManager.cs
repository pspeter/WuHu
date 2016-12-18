using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface ICommonManager
    {
        bool SetScore(Match match, Credentials credentials);
        IList<Match> GetAllMatches();
        IList<Match> GetAllMatchesFor(Player player);
        IList<Match> GetAllMatchesFor(Tournament tournament);
        IList<Match> GetAllUnfinishedMatches();

        bool AddPlayer(Player player, Credentials credentials);
        bool UpdatePlayer(Player player, Credentials credentials);
        bool ChangePassword(string username, string newPassword, Credentials credentials);
        Player GetPlayer(int playerId);
        Player GetPlayer(string username);
        IList<Player> GetAllPlayers();
        IList<Player> GetRanklist();

        bool AddCurrentRatingFor(Player player, Credentials credentials);
        IList<Rating> GetAllRatings();
        IList<Rating> GetAllRatingsFor(Player player);
        Rating GetCurrentRatingFor(Player player);

        IList<ScoreParameter> GetAllParameters();
        bool AddParameter(ScoreParameter param, Credentials credentials);
        bool UpdateParameter(ScoreParameter param, Credentials credentials);
        ScoreParameter GetParameter(string key);

        bool CreateTournament(Tournament tournament, IList<Player> players, int amountMatches, Credentials credentials);
        bool UpdateTournament(Tournament tournament, IList<Player> players, int amountMatches, Credentials credentials);
        bool LockTournament(Credentials credentials);
        void UnlockTournament(Credentials credentials);
    }
}
