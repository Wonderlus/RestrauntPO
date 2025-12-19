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

            _vm.RequestCheckout += OnRequestCheckout;
            Unloaded += BasketPage_Unloaded;
        }

        private void BasketPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _vm.RequestCheckout -= OnRequestCheckout;
        }

        private void OnRequestCheckout()
        {
            var win = new CheckoutWindow(_vm.TotalPrice)
            {
                Owner = Window.GetWindow(this)
            };

            win.ShowDialog();
            _vm.LoadBasket();
        }
    }
}
