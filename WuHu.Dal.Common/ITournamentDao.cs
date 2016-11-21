using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Dal.Common
{
    public interface ITournamentDao
    {
        IList<Tournament> FindAll();
        Tournament FindById(int tournamentId);
        int Insert(Tournament tournament);
        bool Update(Tournament tournament);
        int Count();
    }
}
