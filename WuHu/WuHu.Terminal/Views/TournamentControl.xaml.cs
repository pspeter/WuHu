using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal.Views
{
    public partial class TournamentControl 
    {
        public TournamentControl()
        {
            InitializeComponent();
            DataContext = new TournamentVm();
        }
    }
}
