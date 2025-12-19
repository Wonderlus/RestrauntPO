using BLL;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly AuthService _authService = new();

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

        public ICommand RegisterCommand { get; }

        // Event for successful registration (View closes the window)
        public event Action? RegistrationSuccess;

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand<PasswordBox>(OnRegister);
        }

        private void OnRegister(PasswordBox? passwordBox)
        {
            var password = passwordBox?.Password ?? "";

            if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            var success = _authService.Register(FullName, Phone, Email, password);

            if (!success)
            {
                MessageBox.Show("User with this phone already exists");
                return;
            }

            MessageBox.Show("Registration successful");
            RegistrationSuccess?.Invoke();
        }
    }
}
