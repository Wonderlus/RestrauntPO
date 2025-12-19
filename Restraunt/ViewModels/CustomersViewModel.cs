using BLL;
using CommunityToolkit.Mvvm.Input;
using DAL.Entities;
using Restraunt.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        private readonly UserService _userService = new();

        public ObservableCollection<CustomerEntity> Customers { get; } = new();

        public IRelayCommand<CustomerEntity> ToggleAdminCommand { get; }
        public IRelayCommand<CustomerEntity> DeleteCommand { get; }

        public CustomersViewModel()
        {
            ToggleAdminCommand = new RelayCommand<CustomerEntity>(OnToggleAdmin);
            DeleteCommand = new RelayCommand<CustomerEntity>(OnDelete);

            LoadUsers();
        }

        private void LoadUsers()
        {
            Customers.Clear();
            var users = _userService.GetAllUsers();
            foreach (var user in users)
            {
                Customers.Add(user);
            }
        }

        private void OnToggleAdmin(CustomerEntity? user)
        {
            if (user == null) return;

            // нельзя менять себя
            if (Session.CurrentUser?.Id == user.Id)
            {
                MessageBox.Show("You cannot change your own role");
                return;
            }

            _userService.SetAdmin(user.Id, !user.IsAdmin);
            LoadUsers();
        }

        private void OnDelete(CustomerEntity? user)
        {
            if (user == null) return;

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
