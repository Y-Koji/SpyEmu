using System.Collections.ObjectModel;
using System.Windows;
using WindowView.Model;

namespace WindowView.Command
{
    public class FilterCommandParameter : Freezable
    {
        public ObservableCollection<WindowModel> Windows
        {
            get { return (ObservableCollection<WindowModel>)GetValue(WindowsProperty); }
            set { SetValue(WindowsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Windows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowsProperty =
            DependencyProperty.Register("Windows", typeof(ObservableCollection<WindowModel>), typeof(FilterCommandParameter), new PropertyMetadata(null));
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(FilterCommandParameter), new PropertyMetadata(string.Empty));
        
        protected override Freezable CreateInstanceCore()
        {
            return new FilterCommandParameter();
        }
    }
}
