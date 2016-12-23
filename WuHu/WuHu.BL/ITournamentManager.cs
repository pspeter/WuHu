﻿using System;
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
        Tournament GetMostRecentTournament();
        bool CreateTournament(Tournament tournament, IList<Player> players, int amountMatches, Credentials credentials);
        bool UpdateTournament(Tournament tournament, IList<Player> players, int amountMatches, Credentials credentials);
        bool LockTournament(Credentials credentials);
        void UnlockTournament(Credentials credentials);
    }
}
