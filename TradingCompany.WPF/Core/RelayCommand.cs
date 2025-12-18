using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TradingCompany.WPF.Core
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _cenExecute;

        public RelayCommand(Action<object> execute, Predicate<object> cenExecute=null)
        {
            _execute = execute;
            _cenExecute = cenExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove {  CommandManager.RequerySuggested -= value;}
        }

        public bool CanExecute(object parameter) => _cenExecute == null || _cenExecute(parameter);
        public void Execute(object parameter) =>_execute(parameter);  
    }
}
