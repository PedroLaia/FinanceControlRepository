using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinanceControlDesktop.Commands
{
    public class RelayCommand : ICommand 
    {
        private readonly Action _executar;
        private readonly Func<bool>? _podeExecutar;

        public RelayCommand(Action executar, Func<bool>? podeExecutar = null)
        {
            _executar = executar;
            _podeExecutar = podeExecutar;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            if(_podeExecutar == null)
            {
                return true;
            }
            return _podeExecutar();
        }

        public void Execute(object? parameter)
        {
            _executar();
        }



    }
}
