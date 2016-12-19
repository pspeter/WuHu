using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WuHu.Terminal.ViewModels;
using WuHu.Terminal.Views;
using WuHu.Terminal.Views.Login;

namespace WuHu.Terminal
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            var dialog = new TerminalWindow();
            dialog.Show();
        }
    }
}
