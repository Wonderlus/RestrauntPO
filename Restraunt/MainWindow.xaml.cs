using Models;
using Restraunt.Controls;
using Restraunt.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restraunt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly AppViewModel _appVm = new();
        private MainContent? _menuPage;
        public MainWindow()
        {
            DataContext = _appVm;
            InitializeComponent();

            // Страница по умолчанию
            NavigateToMenu();
        }

        public void Navigate(UserControl page)
        {
            MainContentHost.Content = page;
        }

        public void NavigateToMenu()
        {
            var vm = new MenuViewModel();

            MainContentHost.Content = new MainContent
            {
                DataContext = vm
            };

            SidebarHost.Content = new MenuSidebar
            {
                DataContext = vm
            };
        }
        

        //public void NavigateToOrders()
        //{
        //    Navigate(new OrdersPage());
        //    SidebarHost.SetOrdersSidebar();
        //}


        public void NavigateToBasket()
        {
            MainContentHost.Content = new BasketPage();
            SidebarHost.SetEmptySidebar();
        }

        public void NavigateToCustomers()
        {
            Navigate(new CustomersPage());
            SidebarHost.SetEmptySidebar();
        }

        public void NavigateToAdresses()
        {
            Navigate(new DeliveryAddressesPage());
            SidebarHost.SetEmptySidebar();
        }

        public void NavigateToOrders()
        {
            var vm = new OrdersViewModel();

            MainContentHost.Content = new OrdersPage
            {
                DataContext = vm
            };

            SidebarHost.Content = new OrdersSidebar
            {
                DataContext = vm
            };
        }

    }
}