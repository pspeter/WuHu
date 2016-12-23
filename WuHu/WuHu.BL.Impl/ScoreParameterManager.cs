using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class ScoreParameterManager : IScoreParameterManager
    {
        protected readonly Authenticator Authentication;
        protected readonly IMatchDao MatchDao;
        protected readonly IPlayerDao PlayerDao;
        protected readonly ITournamentDao TournamentDao;
        protected readonly IScoreParameterDao ParamDao;
        protected readonly IRatingDao RatingDao;
        protected readonly Random Rand = new Random();
        protected bool TournamentLocked;

        public ScoreParameterManager()
        {
            var database = DalFactory.CreateDatabase();
            MatchDao = DalFactory.CreateMatchDao(database);
            PlayerDao = DalFactory.CreatePlayerDao(database);
            RatingDao = DalFactory.CreateRatingDao(database);
            TournamentDao = DalFactory.CreateTournamentDao(database);
            ParamDao = DalFactory.CreateScoreParameterDao(database);
            Authentication = Authenticator.GetInstance();
        }
        
        public IList<ScoreParameter> GetAllParameters()
        {
            return ParamDao.FindAll();
        }

        public bool AddParameter(ScoreParameter param, Credentials credentials)
        {
            if (!Authenticate(credentials, true))
            {
                return false;
            }
            return ParamDao.Insert(param);
        }

        public bool UpdateParameter(ScoreParameter param, Credentials credentials)
        {
            if (!Authenticate(credentials, true))
            {
                return false;
            }
            return ParamDao.Update(param);
        }

        public ScoreParameter GetParameter(string key)
        {
            return ParamDao.FindById(key);
        }

        private bool Authenticate(Credentials credentials, bool adminRequired)
        {
            return Authentication?.Authenticate(credentials, adminRequired) ?? false;
        }
    }
}
