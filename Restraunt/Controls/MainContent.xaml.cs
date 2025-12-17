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

            // Загружаем блюда при старте
            LoadDishes();
        }

        private void LoadDishes()
        {
            _allDishes = _dishService.GetMenuDishes();
            DishesList.ItemsSource = _allDishes;
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

        public void ApplyFilters(MenuFilters? filters)
        {
            if (_allDishes == null || !_allDishes.Any())
            {
                LoadDishes();
            }

            if (filters == null)
            {
                DishesList.ItemsSource = _allDishes;
                return;
            }

            IEnumerable<DishModel> result = _allDishes;

            if (filters.Categories.Any())
            {
                result = result.Where(d =>
                    d.CategoryName != null &&
                    filters.Categories.Any(c =>
                        string.Equals(c.Trim(), d.CategoryName.Trim(),
                            StringComparison.OrdinalIgnoreCase)));
            }

            if (filters.OnlySeasonal)
                result = result.Where(d => d.IsSeasonal);

            if (filters.OnlyPromotional)
                result = result.Where(d => d.IsPromotional);

            DishesList.ItemsSource = result.ToList();
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
