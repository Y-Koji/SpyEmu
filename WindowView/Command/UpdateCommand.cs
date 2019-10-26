using SpyEmuCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using WindowView.Model;

namespace WindowView.Command
{
    public class UpdateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public IDictionary<IntPtr, Process> ProcessCache { get; set; } = new Dictionary<IntPtr, Process>();

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (parameter is ObservableCollection<WindowModel> windows)
            {
                windows.Clear();
                var source = CollectionViewSource.GetDefaultView(windows);
                source.SortDescriptions.Clear();
                source.SortDescriptions.Add(new SortDescription("Process.Id", ListSortDirection.Ascending));
                source.SortDescriptions.Add(new SortDescription("Window.Path", ListSortDirection.Ascending));

                var dispatcher = Dispatcher.CurrentDispatcher;
                await Task.Run(() =>
                {
                    IntPtr hWnd = Spy.GetDesktopWindow();
                    foreach (var window in Spy.FindChildWindows(hWnd))
                    {
                        Process process = null;
                        if (ProcessCache.ContainsKey(window.hWnd))
                        {
                            process = ProcessCache[window.hWnd];
                        }
                        else
                        {
                            process = Spy.GetWindowThreadProcess(window.hWnd);
                            ProcessCache.Add(window.hWnd, process);
                        }

                        dispatcher.Invoke(() =>
                        {
                            windows.Add(new WindowModel
                            {
                                Window = window,
                                Process = process,
                            });
                        });
                    }
                });
            }
        }
    }
}
