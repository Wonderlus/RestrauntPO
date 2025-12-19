using BLL;
using CommunityToolkit.Mvvm.Input;
using Models;
using Restraunt.Models;
using Restraunt.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public bool IsAdmin => Session.CurrentUser?.IsAdmin == true;
        private readonly DishService _dishService = new();
        private readonly DishCategoryService _categoryService = new();
        private readonly BasketService _basketService = new();

        // источник истины
        private List<DishModel> _allDishes = new();

        // то, что отображается в UI
        public ObservableCollection<DishModel> Dishes { get; } = new();

        // категории для сайдбара
        public ObservableCollection<CategoryFilterItem> Categories { get; } = new();

        // флаги
        private bool _onlySeasonal;
        public bool OnlySeasonal
        {
            get => _onlySeasonal;
            set
            {
                _onlySeasonal = value;
                OnPropertyChanged();
            }
        }

        private bool _onlyPromotional;
        public bool OnlyPromotional
        {
            get => _onlyPromotional;
            set
            {
                _onlyPromotional = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand AddDishCommand { get; }
        public IRelayCommand<DishModel> EditDishCommand { get; }
        public IRelayCommand<DishModel> DeleteDishCommand { get; }
        public IRelayCommand<DishModel> AddToBasketCommand { get; }

        // Events for dialog requests (View subscribes to these)
        public event Action? RequestAddDish;
        public event Action<DishModel>? RequestEditDish;

        public MenuViewModel()
        {
            // Initialize commands
            ApplyFiltersCommand = new RelayCommand(ApplyFilters);
            ResetFiltersCommand = new RelayCommand(ResetFilters);
            AddDishCommand = new RelayCommand(OnAddDish);
            EditDishCommand = new RelayCommand<DishModel>(OnEditDish);
            DeleteDishCommand = new RelayCommand<DishModel>(OnDeleteDish);
            AddToBasketCommand = new RelayCommand<DishModel>(OnAddToBasket);

            LoadData();
            OnPropertyChanged(nameof(IsAdmin));
        }

        // =========================
        // ЗАГРУЗКА ДАННЫХ
        // =========================
        private void LoadData()
        {
            // грузим блюда
            _allDishes = _dishService.GetMenuDishes().ToList();
            ReplaceDishes(_allDishes);

            // грузим категории
            var cats = _categoryService.GetAll();
            Categories.Clear();

            foreach (var c in cats)
            {
                Categories.Add(new CategoryFilterItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsSelected = false
                });
            }
        }

        // =========================
        // ФИЛЬТРАЦИЯ
        // =========================
        public void ApplyFilters()
        {
            IEnumerable<DishModel> query = _allDishes;

            // фильтр по категориям
            var selectedCategoryIds = Categories
                .Where(x => x.IsSelected)
                .Select(x => x.Id)
                .ToHashSet();

            if (selectedCategoryIds.Count > 0)
            {
                query = query.Where(d =>
                    d.CategoryId.HasValue &&
                    selectedCategoryIds.Contains(d.CategoryId.Value));
            }

            // сезонные
            if (OnlySeasonal)
                query = query.Where(d => d.IsSeasonal);

            // акционные
            if (OnlyPromotional)
                query = query.Where(d => d.IsPromotional);

            ReplaceDishes(query.ToList());
        }

        public void ResetFilters()
        {
            // сброс категорий
            foreach (var c in Categories)
                c.IsSelected = false;

            // сброс флагов
            OnlySeasonal = false;
            OnlyPromotional = false;

            ReplaceDishes(_allDishes);
        }

        // =========================
        // DISH OPERATIONS
        // =========================
        private void OnAddDish()
        {
            if (Session.CurrentUser?.IsAdmin != true)
            {
                MessageBox.Show("Недостаточно прав");
                return;
            }

            RequestAddDish?.Invoke();
        }

        private void OnEditDish(DishModel? dish)
        {
            if (dish == null) return;

            if (Session.CurrentUser?.IsAdmin != true)
            {
                MessageBox.Show("Недостаточно прав");
                return;
            }

            RequestEditDish?.Invoke(dish);
        }

        private void OnDeleteDish(DishModel? dish)
        {
            if (dish == null) return;

            if (Session.CurrentUser?.IsAdmin != true)
            {
                MessageBox.Show("Недостаточно прав");
                return;
            }

            var result = MessageBox.Show(
                $"Удалить блюдо «{dish.Name}»?",
                "Удаление блюда",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                _dishService.Delete(dish.Id);
                Reload();
            }
        }

        private void OnAddToBasket(DishModel? dish)
        {
            if (dish == null) return;

            if (Session.CurrentUser == null)
            {
                MessageBox.Show("Вы не вошли в систему");
                return;
            }

            _basketService.AddDish(Session.CurrentUser.Id, dish.Id);
            MessageBox.Show($"«{dish.Name}» добавлено в корзину");
        }

        // =========================
        // ОБНОВЛЕНИЕ UI
        // =========================
        private void ReplaceDishes(IEnumerable<DishModel> items)
        {
            Dishes.Clear();
            foreach (var d in items)
                Dishes.Add(d);
        }

        public void Reload()
        {
            _allDishes = _dishService.GetMenuDishes().ToList();
            OnPropertyChanged(nameof(IsAdmin));
            ApplyFilters();
        }
    }
}
