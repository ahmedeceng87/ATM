using System.Windows.Controls;
using WithdrawlSchemeA.ViewModels;

namespace WithdrawlSchemeA.Views
{
    /// <summary>
    /// Interaction logic for WithdrawSchemeAView.xaml
    /// </summary>
    public partial class WithdrawSchemeAView : UserControl
    {
        public WithdrawSchemeAView(WithdrawlSchemeAViewModel withdrawlSchemeAViewModel)
        {
            InitializeComponent();
            this.DataContext = withdrawlSchemeAViewModel;
        }
    }
}
