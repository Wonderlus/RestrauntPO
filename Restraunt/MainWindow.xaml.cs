using CommunityToolkit.Mvvm.Messaging;
using Restraunt.Controls;
using Restraunt.Messages;
using Restraunt.Services;
using Restraunt.ViewModels;
using Restraunt.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Restraunt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppViewModel _appVm = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _appVm;

            // Register for navigation messages
            WeakReferenceMessenger.Default.Register<NavigateToPageMessage>(this, (r, m) =>
            {
                switch (m.Value)
                {
                    case "Menu":
                        NavigateToMenu();
                        break;
                    case "Basket":
                        NavigateToBasket();
                        break;
                    case "Orders":
                        NavigateToOrders();
                        break;
                    case "Profile":
                        NavigateToProfile();
                        break;
                    case "Customers":
                        NavigateToCustomers();
                        break;
                    case "Addresses":
                        NavigateToAddresses();
                        break;
                }
            });

            // Register for logout message
            WeakReferenceMessenger.Default.Register<LogoutMessage>(this, (r, m) =>
            {
                Logout();
            });

            // Default page
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

        public void NavigateToAddresses()
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

        public void NavigateToProfile()
        {
            var vm = new ProfileViewModel();

            MainContentHost.Content = new ProfilePage
            {
                DataContext = vm
            };

            SidebarHost.SetEmptySidebar();
        }

        private void Logout()
        {
            // Unregister from messages
            WeakReferenceMessenger.Default.UnregisterAll(this);

            // Clear session
            Session.CurrentUser = null;

            // Open login window
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            // Close current window
            Close();
        }
    }
}
