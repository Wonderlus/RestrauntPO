using Restraunt.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class MenuSidebar : UserControl
    {
        public MenuSidebar()
        {
            InitializeComponent();
            Loaded += MenuSidebar_Loaded;
        }

        private void MenuSidebar_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MenuViewModel vm)
                return;

            // Подключаем категории
            CategoriesList.ItemsSource = vm.Categories;

            // Синхронизация чекбоксов
            SeasonalCheck.IsChecked = vm.OnlySeasonal;
            PromoCheck.IsChecked = vm.OnlyPromotional;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MenuViewModel vm)
                return;

            // Считываем чекбоксы
            vm.OnlySeasonal = SeasonalCheck.IsChecked == true;
            vm.OnlyPromotional = PromoCheck.IsChecked == true;

            // Применяем фильтры
            vm.ApplyFilters();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MenuViewModel vm)
                return;

            // Сбрасываем UI
            SeasonalCheck.IsChecked = false;
            PromoCheck.IsChecked = false;

            // Сбрасываем VM
            vm.ResetFilters();
        }
    }
}
