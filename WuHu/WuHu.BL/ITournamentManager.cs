using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface ITournamentManager
    {
        IList<Tournament> GetAllTournaments();
        Tournament GetTournamentById(int tournamentId);
        Tournament GetMostRecentTournament();
        bool CreateTournament(Tournament tournament, IList<Player> players, int amountMatches);
        bool UpdateTournament(Tournament tournament, IList<Player> players, int amountMatches);
        bool LockTournament();
        void UnlockTournament();
    }
}
