using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Input;
using WuHu.BL.Impl;
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal
{
    public partial class TerminalWindow : Window
    {
        public TerminalWindow()
        {
            InitializeComponent();
            Closing += (a, b) => ManagerFactory.GetTerminalManager().Logout();
        }
    }
}
