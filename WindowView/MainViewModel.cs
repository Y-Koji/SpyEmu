using System.Collections.ObjectModel;
using WindowView.Model;

namespace WindowView
{
    public class MainViewModel
    {
        public ObservableCollection<WindowModel> Windows { get; } = new ObservableCollection<WindowModel>();
    }
}
