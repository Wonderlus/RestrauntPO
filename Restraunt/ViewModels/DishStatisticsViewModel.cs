using BLL;
using CommunityToolkit.Mvvm.Input;
using Restraunt.Models;
using Restraunt.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class DishStatisticsViewModel : ViewModelBase
    {
        private readonly StatisticsService _statisticsService = new();

        public bool IsAdmin => Session.CurrentUser?.IsAdmin == true;

        // Период для статистики
        private DateTime _dateFrom = DateTime.Today.AddDays(-30);
        public DateTime DateFrom
        {
            get => _dateFrom;
            set
            {
                _dateFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dateTo = DateTime.Today;
        public DateTime DateTo
        {
            get => _dateTo;
            set
            {
                _dateTo = value;
                OnPropertyChanged();
            }
        }

        // Статистика по блюдам
        private ObservableCollection<DishStatisticsModel> _dishStatistics = new();
        public ObservableCollection<DishStatisticsModel> DishStatistics
        {
            get => _dishStatistics;
            private set
            {
                _dishStatistics = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasStatistics));
                OnPropertyChanged(nameof(TotalRevenue));
                OnPropertyChanged(nameof(TotalOrders));
            }
        }

        // Статистика по категориям
        private ObservableCollection<CategoryStatisticsModel> _categoryStatistics = new();
        public ObservableCollection<CategoryStatisticsModel> CategoryStatistics
        {
            get => _categoryStatistics;
            private set
            {
                _categoryStatistics = value;
                OnPropertyChanged();
            }
        }

        // Выбранный тип сортировки
        private string _selectedSortType = "По выручке";
        public string SelectedSortType
        {
            get => _selectedSortType;
            set
            {
                _selectedSortType = value;
                OnPropertyChanged();
                ApplySorting();
            }
        }

        public string[] SortTypes { get; } = new[]
        {
            "По выручке",
            "По количеству заказов",
            "По количеству порций",
            "По названию"
        };

        // Выбранный тип статистики (блюда или категории)
        private string _selectedViewType = "Блюда";
        public string SelectedViewType
        {
            get => _selectedViewType;
            set
            {
                _selectedViewType = value;
                OnPropertyChanged();
                LoadStatistics();
            }
        }

        public string[] ViewTypes { get; } = new[]
        {
            "Блюда",
            "Категории"
        };

        // Вычисляемые свойства
        public bool HasStatistics => DishStatistics.Count > 0 || CategoryStatistics.Count > 0;
        
        public decimal TotalRevenue => 
            SelectedViewType == "Блюда" 
                ? DishStatistics.Sum(s => s.TotalRevenue)
                : CategoryStatistics.Sum(s => s.TotalRevenue);
        
        public int TotalOrders => 
            SelectedViewType == "Блюда"
                ? DishStatistics.Sum(s => s.OrderCount)
                : 0; // Для категорий не считаем заказы

        // Commands
        public ICommand LoadStatisticsCommand { get; }
        public ICommand ResetPeriodCommand { get; }
        public ICommand ExportToExcelCommand { get; }

        public DishStatisticsViewModel()
        {
            LoadStatisticsCommand = new RelayCommand(LoadStatistics);
            ResetPeriodCommand = new RelayCommand(ResetPeriod);
            ExportToExcelCommand = new RelayCommand(ExportToExcel, () => HasStatistics);

            LoadStatistics();
        }

        private void LoadStatistics()
        {
            if (SelectedViewType == "Блюда")
            {
                var stats = _statisticsService.GetDishStatistics(DateFrom, DateTo);
                DishStatistics.Clear();
                foreach (var stat in stats)
                {
                    DishStatistics.Add(stat);
                }
            }
            else
            {
                var stats = _statisticsService.GetCategoryStatistics(DateFrom, DateTo);
                CategoryStatistics.Clear();
                foreach (var stat in stats)
                {
                    CategoryStatistics.Add(stat);
                }
            }

            ApplySorting();
            OnPropertyChanged(nameof(HasStatistics));
            OnPropertyChanged(nameof(TotalRevenue));
            OnPropertyChanged(nameof(TotalOrders));
            (ExportToExcelCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }

        private void ApplySorting()
        {
            if (SelectedViewType == "Блюда")
            {
                var sorted = SelectedSortType switch
                {
                    "По выручке" => DishStatistics.OrderByDescending(s => s.TotalRevenue),
                    "По количеству заказов" => DishStatistics.OrderByDescending(s => s.OrderCount),
                    "По количеству порций" => DishStatistics.OrderByDescending(s => s.TotalQuantity),
                    "По названию" => DishStatistics.OrderBy(s => s.DishName),
                    _ => DishStatistics.OrderByDescending(s => s.TotalRevenue)
                };

                var list = sorted.ToList();
                DishStatistics.Clear();
                foreach (var item in list)
                {
                    DishStatistics.Add(item);
                }
            }
            else
            {
                var sorted = SelectedSortType switch
                {
                    "По выручке" => CategoryStatistics.OrderByDescending(s => s.TotalRevenue),
                    "По количеству порций" => CategoryStatistics.OrderByDescending(s => s.TotalQuantity),
                    "По названию" => CategoryStatistics.OrderBy(s => s.CategoryName),
                    _ => CategoryStatistics.OrderByDescending(s => s.TotalRevenue)
                };

                var list = sorted.ToList();
                CategoryStatistics.Clear();
                foreach (var item in list)
                {
                    CategoryStatistics.Add(item);
                }
            }
        }

        private void ResetPeriod()
        {
            DateFrom = DateTime.Today.AddDays(-30);
            DateTo = DateTime.Today;
            LoadStatistics();
        }

        private void ExportToExcel()
        {
            if (!HasStatistics)
                return;

            // TODO: Реализовать экспорт в Excel при необходимости
            // Можно использовать существующий ExcelExportService как пример
        }

        public void Reload()
        {
            LoadStatistics();
        }
    }
}
