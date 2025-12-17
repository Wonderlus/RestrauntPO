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
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.Save())
            {
                DialogResult = true;
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
