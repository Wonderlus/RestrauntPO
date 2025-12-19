using Restraunt.ViewModels;
using System.Windows;

namespace Restraunt.Windows
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _vm = new();

        public LoginWindow()
        {
            InitializeComponent();
            DataContext = _vm;

            _vm.LoginSuccess += OnLoginSuccess;
            _vm.RequestOpenRegister += OnRequestOpenRegister;
        }

        private void OnLoginSuccess()
        {
            // Open main window
            var main = new MainWindow();
            Application.Current.MainWindow = main;
            main.Show();
            Close();
        }

        private void OnRequestOpenRegister()
        {
            var window = new RegisterWindow
            {
                Owner = this
            };

            window.ShowDialog();
        }
    }
}
