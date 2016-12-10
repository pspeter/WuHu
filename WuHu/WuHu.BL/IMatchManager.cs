using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Server
{
    public interface IMatchManager
    {
        bool AddMatch(Match match, Credentials credentials);
        bool UpdateMatch(Match match, Credentials credentials);
    }
}
