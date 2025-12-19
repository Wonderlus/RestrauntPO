using BLL;
using CommunityToolkit.Mvvm.Input;
using Restraunt.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly UserService _userService = new();
        private readonly LoyaltyService _loyaltyService = new();

        private string _fullName = "";
        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        private string _phone = "";
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        private string _email = "";
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public int LoyaltyPoints { get; private set; }

        public ICommand SaveCommand { get; }

        public ProfileViewModel()
        {
            SaveCommand = new RelayCommand(Save);

            if (Session.CurrentUser == null) return;

            FullName = Session.CurrentUser.FullName;
            Phone = Session.CurrentUser.Phone;
            Email = Session.CurrentUser.Email;
            LoadLoyaltyPoints();
        }

        private void LoadLoyaltyPoints()
        {
            if (Session.CurrentUser == null) return;
            LoyaltyPoints = _loyaltyService.GetCustomerPoints(Session.CurrentUser.Id);
            OnPropertyChanged(nameof(LoyaltyPoints));
        }

        public void Save()
        {
            if (Session.CurrentUser == null)
                return;

            if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            try
            {
                _userService.UpdateUser(
                    Session.CurrentUser.Id,
                    FullName,
                    Phone,
                    Email
                );

                // обновляем текущую сессию
                Session.CurrentUser.FullName = FullName;
                Session.CurrentUser.Phone = Phone;
                Session.CurrentUser.Email = Email;
                // Обновляем баллы в сессии
                if (Session.CurrentUser != null)
                {
                    Session.CurrentUser.LoyaltyPoints = _loyaltyService.GetCustomerPoints(Session.CurrentUser.Id);
                }
                MessageBox.Show("Профиль сохранен");
                // Уведомляем AppViewModel
                Session.NotifyUserUpdated();
                LoadLoyaltyPoints();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
