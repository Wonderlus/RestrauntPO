using Restraunt.ViewModels;
using System.Windows;

namespace Restraunt.Windows
{
    public partial class RegisterWindow : Window
    {
        private readonly RegisterViewModel _vm = new();

        public RegisterWindow()
        {
            InitializeComponent();
            DataContext = _vm;

            _vm.RegistrationSuccess += OnRegistrationSuccess;
        }

        private void OnRegistrationSuccess()
        {
            Close();
        }
    }
}
