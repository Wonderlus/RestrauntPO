using Restraunt.Services;
using Restraunt.Windows;
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
    /// Логика взаимодействия для TopNavBar.xaml
    /// </summary>
    public partial class TopNavBar : UserControl
    {
        public TopNavBar()
        {
            InitializeComponent();
        }
        private MainWindow? Main =>
            Window.GetWindow(this) as MainWindow;

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Main?.NavigateToMenu();
        }

        //private void Orders_Click(object sender, RoutedEventArgs e)
        //{
        //    Main?.NavigateToOrders();
        //}

        private void Basket_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow main)
                main.NavigateToBasket();
        }
        private void Addresses_Click(object sender, RoutedEventArgs e)
        {
            Main?.NavigateToAdresses();
        }
        private void Customers_Click(object sender, RoutedEventArgs e)
        {
            Main?.NavigateToCustomers();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // 1️⃣ Очищаем сессию
            Session.CurrentUser = null;

            // 2️⃣ Открываем окно логина
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            // 3️⃣ Закрываем текущее главное окно
            Window.GetWindow(this)?.Close();
        }
    }
}
