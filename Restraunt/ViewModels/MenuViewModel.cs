using Restraunt.Models;  
using BLL;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Restraunt.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly DishService _dishService;

        public ObservableCollection<DishModel> Dishes { get; set; }

        public ICommand ShowAllDishesCommand { get; set; }
        public ICommand ShowSeasonalDishesCommand { get; set; }
        public ICommand ShowPromotionalDishesCommand { get; set; }

        public MenuViewModel()
        {
            _dishService = new DishService();

            // Изначально показываем все блюда
            Dishes = new ObservableCollection<DishModel>(
                _dishService.GetMenuDishes()
            );

            // Команды для фильтрации
            ShowAllDishesCommand = new RelayCommand(ShowAllDishes);
            ShowSeasonalDishesCommand = new RelayCommand(ShowSeasonalDishes);
            ShowPromotionalDishesCommand = new RelayCommand(ShowPromotionalDishes);
        }

        private void ShowAllDishes()
        {
            Dishes.Clear();
            var allDishes = _dishService.GetMenuDishes();
            foreach (var dish in allDishes)
            {
                Dishes.Add(dish);
            }
        }

        private void ShowSeasonalDishes()
        {
            Dishes.Clear();
            var seasonalDishes = _dishService.GetMenuDishes()
                .Where(d => d.IsSeasonal)
                .ToList();

            foreach (var dish in seasonalDishes)
            {
                Dishes.Add(dish);
            }
        }

        private void ShowPromotionalDishes()
        {
            Dishes.Clear();
            var promotionalDishes = _dishService.GetMenuDishes()
                .Where(d => d.IsPromotional)
                .ToList();

            foreach (var dish in promotionalDishes)
            {
                Dishes.Add(dish);
            }
        }
    }
}
