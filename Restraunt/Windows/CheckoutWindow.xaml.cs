using Restraunt.ViewModels;
using System.Windows;

namespace Restraunt.Windows
{
    public partial class CheckoutWindow : Window
    {
        public CheckoutWindow(decimal totalAmount)
        {
            InitializeComponent();

            var vm = new CheckoutViewModel(totalAmount);
            vm.OrderCreated += () => Close();
            vm.Cancelled += () => Close();

            DataContext = vm;
        }
    }
}
