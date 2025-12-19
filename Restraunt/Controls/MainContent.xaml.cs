using Restraunt.Models;
using Restraunt.ViewModels;
using Restraunt.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class MainContent : UserControl
    {
        public MainContent()
        {
            InitializeComponent();
            Loaded += MainContent_Loaded;
            Unloaded += MainContent_Unloaded;
        }

        private void MainContent_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
            {
                vm.RequestAddDish += OnRequestAddDish;
                vm.RequestEditDish += OnRequestEditDish;
            }
        }

        private void MainContent_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
            {
                vm.RequestAddDish -= OnRequestAddDish;
                vm.RequestEditDish -= OnRequestEditDish;
            }
        }

        private void OnRequestAddDish()
        {
            var window = new DishEditWindow { Owner = Window.GetWindow(this) };

            if (window.ShowDialog() == true && DataContext is MenuViewModel vm)
                vm.Reload();
        }

        private void OnRequestEditDish(DishModel dish)
        {
            var window = new DishEditWindow(dish) { Owner = Window.GetWindow(this) };

            if (window.ShowDialog() == true && DataContext is MenuViewModel vm)
                vm.Reload();
        }
    }
}
