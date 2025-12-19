using BLL;
using CommunityToolkit.Mvvm.Input;
using Restraunt.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthService _authService = new();

        private string _phone = "";
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenRegisterCommand { get; }

        // Events for View to handle
        public event Action? LoginSuccess;
        public event Action? RequestOpenRegister;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<PasswordBox>(Login);
            OpenRegisterCommand = new RelayCommand(OnOpenRegister);
        }

        private void Login(PasswordBox? passwordBox)
        {
            var password = passwordBox?.Password ?? "";

            var user = _authService.Login(Phone, password);
            if (user == null)
            {
                MessageBox.Show("Неверный телефон или пароль");
                return;
            }

            Session.CurrentUser = user;
            LoginSuccess?.Invoke();
        }

        private void OnOpenRegister()
        {
            RequestOpenRegister?.Invoke();
        }
    }
}
