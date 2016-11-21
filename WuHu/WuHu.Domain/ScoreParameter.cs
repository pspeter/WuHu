using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
    [Serializable]
    public class ScoreParameter
    {
        public ScoreParameter() { }
        public ScoreParameter(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
        
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
