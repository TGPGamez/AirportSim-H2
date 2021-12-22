using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp.MVVM.Commands
{
    public class BaseCommand : ICommand
    {
        private Action<object> method;
        public BaseCommand(Action<object> method)
        {
            this.method = method;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            method.Invoke(parameter);
        }
    }
}
