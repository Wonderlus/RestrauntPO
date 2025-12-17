using BLL;
using Restraunt.Services;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
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

        private string _password = "";
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            var authService = new AuthService();

            var user = authService.Login(Phone, Password);
            if (user == null)
            {
                MessageBox.Show("Неверные данные");
                return;
            }

            Session.CurrentUser = user;
        }
    }
}
