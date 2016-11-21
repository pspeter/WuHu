using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Dal.Common
{
    interface IScoreParameterDao
    {
        IList<ScoreParameter> FindAll();
        ScoreParameter FindById(string key);
        int Insert(ScoreParameter param);
        bool Update(ScoreParameter param);
    }
}
