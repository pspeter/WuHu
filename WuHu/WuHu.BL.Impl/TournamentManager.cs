using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    class TournamentManager : ITournamentManager
    {
        private static TournamentManager _instance;
        private readonly Authenticator _authenticator;
        private readonly ITournamentDao _tournamentDao;

        protected TournamentManager()
        {
            var database = DalFactory.CreateDatabase();
            _tournamentDao = DalFactory.CreateTournamentDao(database);
            _authenticator = Authenticator.GetInstance();
        }

        public static TournamentManager GetInstance()
        {
            return _instance ?? (_instance = new TournamentManager());
        }

        public bool CreateTournament(Tournament tournament, IList<Player> players, int amountMatches, Credentials credentials)
        {
            if (!Authenticate(credentials, true))
            {
                return false;
            }
            var inserted = _tournamentDao.Insert(tournament);
            if (!inserted)
            {
                return false;
            }

            MatchManager.GetInstance().CreateMatches(tournament, players, amountMatches, credentials);
            return true;
        }

        public bool UpdateTournament(Tournament tournament, IList<Player> players, int amountMatches, Credentials credentials)
        {
            return MatchManager.GetInstance().CreateMatches(tournament, players, amountMatches, credentials);
        }

        private bool Authenticate(Credentials credentials, bool adminRequired)
        {
            return _authenticator.Authenticate(credentials, adminRequired);
        }
    }
}
