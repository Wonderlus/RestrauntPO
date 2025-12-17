using Restraunt.ViewModels;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class OrdersSidebar : UserControl
    {
        public OrdersSidebar()
        {
            InitializeComponent();
        }

        private void Reset_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is not OrdersViewModel vm)
                return;

            vm.SelectedStatus = "Все";
            vm.SelectedOrderType = "Все";
        }
    }
}
