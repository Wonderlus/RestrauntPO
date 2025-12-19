using Restraunt.ViewModels;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class CustomersPage : UserControl
    {
        public CustomersPage()
        {
            InitializeComponent();
            DataContext = new CustomersViewModel();
        }
    }
}
