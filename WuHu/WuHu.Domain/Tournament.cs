using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
    [Serializable]
    public class Tournament
    {

        public Tournament() { }

        public Tournament(string name, Player creator)
        {
            this.Name = name;
            this.Creator = creator;
        }
       
        public string Name { get;  set; }
        public Player Creator { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
