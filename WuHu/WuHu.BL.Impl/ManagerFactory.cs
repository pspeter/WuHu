using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.BL.Impl
{
    public static class ManagerFactory
    {
        private static ITerminalManager _terminalManager;

        public static ITerminalManager GetTerminalManager()
        {
            return _terminalManager ?? (_terminalManager = new TerminalManager());
        }
    }
}
