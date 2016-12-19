using System.Runtime.Remoting.Contexts;
using System.Windows;
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal
{
    public partial class TerminalWindow : Window
    {
        public TerminalWindow()
        {
            InitializeComponent();
            DataContext = new PlayerCollectionVm();
        }
    }
}
