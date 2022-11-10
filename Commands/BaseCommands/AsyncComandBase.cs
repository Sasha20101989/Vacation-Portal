using System.Threading.Tasks;

namespace Vacation_Portal.Commands.BaseCommands
{
    public abstract class ComandBase : CommandBase {

        private bool isExecuting;

        private bool IsExecuting {
            get { return isExecuting; }
            set { isExecuting = value; OnCanExecutedChanged (); }
        }

        public override bool CanExecute (object parameter) {
            return !IsExecuting && base.CanExecute (parameter);
        }

        public override async void Execute (object parameter) {
            IsExecuting = true;
            try {
                await ExecuteAsync (parameter);
            } finally {
                IsExecuting = false;
            }
        }
        public abstract Task ExecuteAsync (object parameter);

    }
}
