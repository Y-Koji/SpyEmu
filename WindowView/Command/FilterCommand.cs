using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using WindowView.Model;

namespace WindowView.Command
{
    public class FilterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is FilterCommandParameter objParam && objParam.Windows != null)
            {
                var source = CollectionViewSource.GetDefaultView(objParam.Windows);
                source.Filter = window =>
                {
                    return ((WindowModel)window).Process.ProcessName.ToLower().Contains(objParam.Text.ToLower());
                };
            }
        }
    }
}
