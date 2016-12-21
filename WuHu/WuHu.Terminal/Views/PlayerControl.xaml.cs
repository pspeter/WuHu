using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal.Views
{
    public partial class PlayerControl
    {
        public PlayerControl()
        {
            InitializeComponent();
            DataContext = new PlayerCollectionVm();
        }
    }
}
