using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Input;
using WuHu.BL.Impl;
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal
{
    public partial class TerminalWindow
    {
        public TerminalWindow()
        {
            InitializeComponent();
            var vm = new TerminalVm();
            DataContext = vm;
            Closing += vm.OnWindowClosing;
        }
    }
}
