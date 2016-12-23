using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface IRatingManager
    {
        bool AddCurrentRatingFor(Player player, Credentials credentials);
        IList<Rating> GetAllRatings();
        IList<Rating> GetAllRatingsFor(Player player);
        Rating GetCurrentRatingFor(Player player);
    }
}
