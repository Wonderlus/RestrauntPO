using Restraunt.ViewModels;
using Restraunt.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class OrdersPage : UserControl
    {
        public OrdersPage()
        {
            InitializeComponent();
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not OrdersViewModel vm)
                return;

            var result = MessageBox.Show(
                "Вы действительно хотите отменить заказ?",
                "Отмена заказа",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
                vm.CancelSelectedOrder();
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not OrdersViewModel vm || vm.SelectedOrder == null)
                return;

            var win = new EditOrderWindow(vm.SelectedOrder)
            {
                Owner = Window.GetWindow(this)
            };

            if (win.ShowDialog() == true)
                vm.Reload();
        }

        private void ExportOrders_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is OrdersViewModel vm)
                vm.ExportOrdersToExcel();
        }
    }
}
