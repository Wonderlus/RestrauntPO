using DAL.Entities;
using Restraunt.ViewModels;
using System.Windows;

namespace Restraunt.Windows
{
    public partial class EditOrderWindow : Window
    {
        private readonly EditOrderViewModel _vm;

        public EditOrderWindow(OrderEntity order)
        {
            InitializeComponent();
            _vm = new EditOrderViewModel(order);
            DataContext = _vm;

            // Set up the close callback
            _vm.RequestClose = result =>
            {
                DialogResult = result;
                Close();
            };
        }
    }
}
