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
                MessageBox.Show("Invalid phone or password");
                return;
            }
            Session.CurrentUser = user;
            // TODO: сохранить текущего пользователя (Session)
            var main = new MainWindow();
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
