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
        private readonly IMatchDao _matchDao;
        private readonly IScoreParameterDao _paramDao;
        protected RatingManager()
        {
            var database = DalFactory.CreateDatabase();
            _ratingDao = DalFactory.CreateRatingDao(database);
            _paramDao = DalFactory.CreateScoreParameterDao(database);
            _matchDao = DalFactory.CreateMatchDao(database);
            _authenticator = Authenticator.GetInstance();
        }
        public static RatingManager GetInstance()
        {
            return _instance ?? (_instance = new RatingManager());
        }

        public bool AddCurrentRatingFor(Player player, Credentials credentials)
        {
            var kRating = int.Parse(_paramDao.FindById("k-rating").Value);
            var halflife = int.Parse(_paramDao.FindById("halflife").Value);
            //var scoredMatches = int.Parse(_paramDao.FindById("scoredMatches").Value);
            var initialScore = int.Parse(_paramDao.FindById("initialScore").Value);

            if (_ratingDao.FindCurrentRating(player) == null)
            {
                _ratingDao.Insert(new Rating(player, DateTime.Now, initialScore));
                return true;
            }

            var matches = _matchDao.FindAllByPlayer(player)
                .Where(m => m.IsDone);



            return false;
        }

        public IList<Rating> GetAllRatings()
        {
            return _ratingDao.FindAll();
        }

        public IList<Rating> GetAllRatingsFor(Player player)
        {
            return _ratingDao.FindAllByPlayer(player);
        }

        public Rating GetCurrentRatingFor(Player player)
        {
            return _ratingDao.FindCurrentRating(player);
        }
    }
}
