using Restraunt.ViewModels;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class OrdersPage : UserControl
    {
        public OrdersPage()
        {
            InitializeComponent();
            DataContext = new OrdersViewModel();
        }
    }
}
