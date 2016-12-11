using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Dal.SqlServer;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class RatingManager : IRatingManager
    {
        private static RatingManager _instance;
        private Authenticator _authenticator;
        private readonly IRatingDao _ratingDao;
        private readonly IScoreParameterDao _paramDao;
        protected RatingManager()
        {
            var database = DalFactory.CreateDatabase();
            _ratingDao = DalFactory.CreateRatingDao(database);
            _paramDao = DalFactory.CreateScoreParameterDao(database);
            _authenticator = Authenticator.GetInstance();
        }
        public static RatingManager GetInstance()
        {
            return _instance ?? (_instance = new RatingManager());
        }

        public bool AddCurrentRatingFor(Player player, Credentials credentials)
        {
            throw new NotImplementedException();
        }

        public IList<Rating> GetAllRatings()
        {
            throw new NotImplementedException();
        }

        public IList<Rating> GetAllRatingsFor(Player player)
        {
            throw new NotImplementedException();
        }

        public Rating GetCurrentRatingFor(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
