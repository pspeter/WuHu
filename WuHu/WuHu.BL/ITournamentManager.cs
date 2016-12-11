using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL
{
    interface ITournamentManager
    {
        bool CreateTournament(string name, IList<Player> players, int amountMatches, Credentials credentials);
        bool UpdateTournament(IList<Player> players, int amountMatches);
    }
}
