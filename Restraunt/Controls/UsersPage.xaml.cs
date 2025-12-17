using BLL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restraunt.Controls
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : UserControl
    {
        public ObservableCollection<CustomerEntity> Users { get; set; }

        private readonly UserService _userService = new();

        public UsersPage()
        {
            InitializeComponent();

            Users = new ObservableCollection<CustomerEntity>(
                _userService.GetAllUsers()
            );

            DataContext = this;
        }
    }
}
