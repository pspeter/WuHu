using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface IScoreParameterManager
    {
        IList<ScoreParameter> GetAllParameters();
        bool AddParameter(ScoreParameter param, Credentials credentials);
        bool UpdateParameter(ScoreParameter param, Credentials credentials);
        ScoreParameter GetParameter(string key);
    }
}
