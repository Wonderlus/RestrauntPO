using Models;
using Restraunt.ViewModels;
using Restraunt.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class BasketPage : UserControl
    {
        private readonly BasketViewModel _vm = new();

        public BasketPage()
        {
            InitializeComponent();
            DataContext = _vm;
            _vm.LoadBasket();
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.Items.Count == 0) return;

            var win = new CheckoutWindow(_vm.TotalPrice)
            {
                Owner = Window.GetWindow(this)
            };

            win.ShowDialog();
            _vm.LoadBasket();
        }
    }
}
