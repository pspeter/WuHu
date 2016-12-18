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
            if (_terminalManager == null)
            {
                _terminalManager = new TerminalManager();
            }
            return _terminalManager;
        }
    }
}
