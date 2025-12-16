using BLL;
using Restraunt.Models;
using Restraunt.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class MainContent : UserControl
    {
        private readonly DishService _dishService;

        public MainContent()
        {
            InitializeComponent();

            _dishService = new DishService();

            // Загружаем блюда при старте
            LoadDishes();
        }

        private void LoadDishes()
        {
            DishesList.ItemsSource = _dishService.GetMenuDishes();
        }

        /// <summary>
        /// Нажатие на кнопку редактирования блюда
        /// </summary>
        private void EditDish_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is DishModel dish)
            {
                var window = new DishEditWindow(dish)
                {
                    Owner = Window.GetWindow(this)
                };

                if (window.ShowDialog() == true)
                    LoadDishes();
            }
        }

        /// <summary>
        /// Нажатие на кнопку удаления блюда
        /// </summary>
        private void DeleteDish_Click(object sender, RoutedEventArgs e)
        {
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
                    LoadDishes();
                }
            }
        }

        /// <summary>
        /// Добавление нового блюда
        /// </summary>
        private void AddDish_Click(object sender, RoutedEventArgs e)
        {
            var window = new DishEditWindow
            {
                Owner = Window.GetWindow(this)
            };

            if (window.ShowDialog() == true)
                LoadDishes();
        }

    }
}
