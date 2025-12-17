using Restraunt.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class ProfilePage : UserControl
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileViewModel vm)
                vm.Save();
        }
    }
}
