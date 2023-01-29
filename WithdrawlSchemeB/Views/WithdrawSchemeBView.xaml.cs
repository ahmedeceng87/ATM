using System.Windows.Controls;
using WithdrawlSchemeB.ViewModels;

namespace WithdrawlSchemeB.Views
{
    /// <summary>
    /// Interaction logic for WithdrawSchemeBView.xaml
    /// </summary>
    public partial class WithdrawSchemeBView : UserControl
    {
        public WithdrawSchemeBView(WithdrawlSchemeBViewModel withdrawlSchemeBViewModel)
        {
            this.DataContext = withdrawlSchemeBViewModel;
            InitializeComponent();
        }
    }
}
