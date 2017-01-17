using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
    [DataContract(Namespace = "http://WuHu.Domain")]
    public class ScoreParameter
    {
        public ScoreParameter() { }
        public ScoreParameter(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        [DataMember]
        [Required]
        public string Key { get; set; }
        [DataMember]
        [Required]
        public string Value { get; set; }
    }
}
