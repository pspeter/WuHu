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
        bool AddParameter(ScoreParameter param);
        bool UpdateParameter(ScoreParameter param);
        ScoreParameter GetParameter(string key);
    }
}
