using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.BL.Test
{
    class TestHelper
    {
        internal static string GenerateName()
        {
            return Guid.NewGuid().ToString().Substring(0, 20); //random 20 character string
        }
    }
}
