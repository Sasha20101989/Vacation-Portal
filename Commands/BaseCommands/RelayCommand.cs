using System;
using System.Windows.Input;

namespace Vacation_Portal.Commands.BaseCommands {
    public class RelayCommand : ICommand {
        private readonly Action<object> _handler;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> handler, Predicate<object> canExecute) {
            _handler = handler;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute(parameter);
        }

        public void Execute(object parameter) {
            _handler(parameter);
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
