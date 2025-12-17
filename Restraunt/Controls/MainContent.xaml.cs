using BLL;
using Models;
using Restraunt.Models;
using Restraunt.Services;
using Restraunt.ViewModels;
using Restraunt.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class MainContent : UserControl
    {
        private List<DishModel> _allDishes = new();

        private readonly DishService _dishService;
        private readonly BasketService _basketService = new();

        public MainContent()
        {
            InitializeComponent();
            _dishService = new DishService();

        }



        /// <summary>
        /// Нажатие на кнопку редактирования блюда
        /// </summary>
        private void EditDish_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser?.IsAdmin != true)
            {
                MessageBox.Show("Недостаточно прав");
                return;
            }

            if (sender is Button button && button.Tag is DishModel dish)
            {
                var window = new DishEditWindow(dish) { Owner = Window.GetWindow(this) };

                if (window.ShowDialog() == true && DataContext is MenuViewModel vm)
                    vm.Reload();
            }
        }


        /// <summary>
        /// Нажатие на кнопку удаления блюда
        /// </summary>
        private void DeleteDish_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser?.IsAdmin != true)
            {
                MessageBox.Show("Недостаточно прав");
                return;
            }

            if (sender is Button button && button.Tag is DishModel dish)
            {
                var result = MessageBox.Show(
                    $"Удалить блюдо «{dish.Name}»?",
                    "Удаление блюда",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    _dishService.Delete(dish.Id);

                    if (DataContext is MenuViewModel vm)
                        vm.Reload();
                }
            }
        }


        /// <summary>
        /// Добавление нового блюда
        /// </summary>
        private void AddDish_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser?.IsAdmin != true)
            {
                MessageBox.Show("Недостаточно прав");
                return;
            }

            var window = new DishEditWindow { Owner = Window.GetWindow(this) };

            if (window.ShowDialog() == true && DataContext is MenuViewModel vm)
                vm.Reload();
        }




        private void AddToBasket_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DishModel dish)
            {
                if (Session.CurrentUser == null)
                {
                    MessageBox.Show("Вы не вошли в систему");
                    return;
                }

                _basketService.AddDish(Session.CurrentUser.Id, dish.Id);
                MessageBox.Show($"«{dish.Name}» добавлено в корзину");
            }
        }

    }
}
