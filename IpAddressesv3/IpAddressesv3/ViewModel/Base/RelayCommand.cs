using System;
using System.Windows.Input;

namespace IpAddressesv3
{
    public class RelayCommand : ICommand
    {
        private Action _Action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action action)
        {
            _Action = action;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _Action();
        }
    }
}
