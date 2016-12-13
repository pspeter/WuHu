using System;
using System.Collections.Generic;
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
