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
        private static ScoreParameterManager _instance;
        private readonly Authenticator _authenticator;
        private readonly IScoreParameterDao _paramDao;

        protected ScoreParameterManager()
        {
            var database = DalFactory.CreateDatabase();
            _paramDao = DalFactory.CreateScoreParameterDao(database);
            _authenticator = Authenticator.GetInstance();
        }

        public static ScoreParameterManager GetInstance()
        {
            return _instance ?? (_instance = new ScoreParameterManager());
        }

        public IList<ScoreParameter> GetAllParameters()
        {
            return _paramDao.FindAll();
        }

        public bool AddParameter(ScoreParameter param, Credentials credentials)
        {
            if (!Authenticate(credentials, true))
            {
                return false;
            }
            return _paramDao.Insert(param);
        }

        public bool UpdateParameter(ScoreParameter param, Credentials credentials)
        {
            if (!Authenticate(credentials, true)) 
            {
                return false;
            }
            return _paramDao.Update(param);
        }

        public ScoreParameter GetParameterFor(string key)
        {
            return _paramDao.FindById(key);
        }
        private bool Authenticate(Credentials credentials, bool adminRequired)
        {
            return _authenticator.Authenticate(credentials, adminRequired);
        }
    }
}
