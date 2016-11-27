using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Dal.Common
{
    public interface IScoreParameterDao
    {
        IList<ScoreParameter> FindAll();
        ScoreParameter FindById(string key);
        bool Insert(ScoreParameter param);
        bool Update(ScoreParameter param);
    }
}
