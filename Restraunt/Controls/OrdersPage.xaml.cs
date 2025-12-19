using DAL.Entities;
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
            Loaded += OrdersPage_Loaded;
            Unloaded += OrdersPage_Unloaded;
        }

        private void OrdersPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is OrdersViewModel vm)
            {
                vm.RequestEditOrder += OnRequestEditOrder;
            }
        }

        private void OrdersPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is OrdersViewModel vm)
            {
                vm.RequestEditOrder -= OnRequestEditOrder;
            }
        }

        private void OnRequestEditOrder(OrderEntity order)
        {
            var win = new EditOrderWindow(order)
            {
                Owner = Window.GetWindow(this)
            };

            if (win.ShowDialog() == true && DataContext is OrdersViewModel vm)
                vm.Reload();
        }
    }
}
