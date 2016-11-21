using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Dal.Common
{
    interface IRatingDao
    {
        IList<Rating> FindAll();
        Rating FindById(int ratingId);
        int Insert(Rating rating);
        bool Update(Rating rating);
    }
}
