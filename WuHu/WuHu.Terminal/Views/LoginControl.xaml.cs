using System.Windows.Controls;
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal.Views
{
    public partial class LoginControl
    {
        public LoginControl()
        {
            InitializeComponent();
            DataContext = new LoginVm();
        }
    }
}
