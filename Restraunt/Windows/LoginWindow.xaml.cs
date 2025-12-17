using BLL;
using Restraunt.Services;
using System.Windows;

namespace Restraunt.Windows
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var phone = PhoneBox.Text;
            var password = PasswordBox.Password;

            var user = _authService.Login(phone, password);

            if (user == null)
            {
                MessageBox.Show("Неверный телефон или пароль");
                return;
            }

            Session.CurrentUser = user;

            // обновляем AppViewModel
            if (Application.Current.MainWindow?.DataContext is AppViewModel appVm)
                appVm.RefreshUser();

            var main = new MainWindow();

            // ВОТ ЭТО ВАЖНО
            Application.Current.MainWindow = main;

            main.Show();
            Close();
        }

        private void OpenRegister_Click(object sender, RoutedEventArgs e)
        {
            var window = new RegisterWindow
            {
                Owner = this
            };

            window.ShowDialog();
        }
    }
}
