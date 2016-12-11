using System;
using System.Collections.Generic;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface IScoreParameterManager
    {
        IList<ScoreParameter> GetAllParameters();
        bool AddParameter(ScoreParameter param);
        ScoreParameter UpdateParameter(ScoreParameter param);
        ScoreParameter GetParameterFor(string key);
    }
}
