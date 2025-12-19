using Restraunt.Models;
using Restraunt.ViewModels;
using System.Windows;

namespace Restraunt.Windows
{
    public partial class DishEditWindow : Window
    {
        private readonly DishEditViewModel _vm;

        // CREATE
        public DishEditWindow()
        {
            InitializeComponent();
            _vm = new DishEditViewModel();
            DataContext = _vm;
            SetupCloseCallback();
        }

        // EDIT
        public DishEditWindow(DishModel dish)
        {
            InitializeComponent();
            _vm = new DishEditViewModel(dish);
            DataContext = _vm;
            SetupCloseCallback();
        }

        private void SetupCloseCallback()
        {
            _vm.RequestClose = result =>
            {
                DialogResult = result;
                Close();
            };
        }
    }
}
