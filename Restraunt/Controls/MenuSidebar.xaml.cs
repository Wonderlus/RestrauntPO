using BLL;
using DAL.Entities;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restraunt.Controls
{
    /// <summary>
    /// Логика взаимодействия для MenuSidebar.xaml
    /// </summary>
    public partial class MenuSidebar : UserControl
    {
        private readonly DishCategoryService _categoryService = new();

        private List<CategoryFilterItem> _categories = new();

        public MenuSidebar()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            _categories = _categoryService.GetAll()
                .Select(c => new CategoryFilterItem
                {
                    Name = c.Name,
                    IsSelected = false
                })
                .ToList();

            CategoriesList.ItemsSource = _categories;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var filters = new MenuFilters
            {
                Categories = _categories
                    .Where(c => c.IsSelected)
                    .Select(c => c.Name)
                    .ToList(),

                OnlySeasonal = SeasonalCheck.IsChecked == true,
                OnlyPromotional = PromoCheck.IsChecked == true
            };

            if (Window.GetWindow(this) is MainWindow main)
                main.ApplyMenuFilters(filters);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            foreach (var cat in _categories)
                cat.IsSelected = false;

            SeasonalCheck.IsChecked = false;
            PromoCheck.IsChecked = false;

            CategoriesList.Items.Refresh();

            if (Window.GetWindow(this) is MainWindow main)
                main.ApplyMenuFilters(null);
        }
    }
}
