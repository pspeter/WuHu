using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Dal.Common
{
    public interface IRatingDao
    {
        IList<Rating> FindAll();
        IList<Rating> FindAllByPlayer(Player player);
        Rating FindById(int ratingId);
        Rating FindCurrentRating(Player player);
        bool Insert(Rating rating);
        bool Update(Rating rating);
        int Count();
    }
}
