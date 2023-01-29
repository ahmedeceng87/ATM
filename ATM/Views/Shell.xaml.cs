using System.Windows;
using ATM.ViewModels;
using Microsoft.Practices.Prism.Events;


namespace ATM
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell(IEventAggregator eventAggregator)
        {
            this.DataContext = new ShellViewModel(eventAggregator);
            InitializeComponent();
        }
    }
}
