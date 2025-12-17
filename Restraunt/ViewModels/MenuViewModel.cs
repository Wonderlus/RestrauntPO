using BLL;
using Models;
using Restraunt.Models;
using Restraunt.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace Restraunt.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {

        public bool IsAdmin => Session.CurrentUser?.IsAdmin == true;
        private readonly DishService _dishService = new();
        private readonly DishCategoryService _categoryService = new();

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

        public MenuViewModel()
        {
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
