using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WuHu.Terminal.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _executeAction;
        private readonly Predicate<object> _canExecutePredicate;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _executeAction = execute;
            _canExecutePredicate = canExecute;
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecutePredicate?.Invoke(parameter) ?? true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
