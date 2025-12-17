using System.Windows;
using BLL;

namespace Restraunt.Windows
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthService _authService = new();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var success = _authService.Register(
                NameBox.Text,
                PhoneBox.Text,
                EmailBox.Text,
                PasswordBox.Password
            );

            if (!success)
            {
                MessageBox.Show("User with this phone already exists");
                return;
            }

            MessageBox.Show("Registration successful");
            Close();
        }
    }
}
