using BLL;
using DAL.Entities;
using Restraunt.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : UserControl
    {
        private readonly UserService _userService = new();

        public ObservableCollection<CustomerEntity> Customers { get; set; }

        public CustomersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            Customers = new ObservableCollection<CustomerEntity>(_userService.GetAllUsers());
            CustomersGrid.ItemsSource = Customers;
        }

        private void ToggleAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CustomerEntity user)
            {
                // нельзя менять себя
                if (Session.CurrentUser?.Id == user.Id)
                {
                    MessageBox.Show("You cannot change your own role");
                    return;
                }

                _userService.SetAdmin(user.Id, !user.IsAdmin);
                LoadUsers();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CustomerEntity user)
            {
                if (Session.CurrentUser?.Id == user.Id)
                {
                    MessageBox.Show("You cannot delete yourself");
                    return;
                }

                var result = MessageBox.Show(
                    $"Delete user {user.FullName}?",
                    "Confirm",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _userService.Delete(user.Id);
                    LoadUsers();
                }
            }
        }
    }
}
