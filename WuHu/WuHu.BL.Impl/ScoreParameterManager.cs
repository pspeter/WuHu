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
        private Authenticator _authenticator;
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

        public bool AddParameter(ScoreParameter param)
        {
            throw new NotImplementedException();
        }

        public ScoreParameter UpdateParameter(ScoreParameter param)
        {
            throw new NotImplementedException();
        }

        public ScoreParameter GetParameterFor(string key)
        {
            return _paramDao.FindById(key);
        }
    }
}
