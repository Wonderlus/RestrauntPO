using BLL;
using DAL.Entities;
using Restraunt.Services;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для DeliveryAddressesPage.xaml
    /// </summary>
    public partial class DeliveryAddressesPage : UserControl
    {
        private readonly DeliveryAddressService _service = new();
        private List<DeliveryAddressEntity> _addresses = new();

        public DeliveryAddressesPage()
        {
            InitializeComponent();
            LoadAddresses();
        }

        private void LoadAddresses()
        {
            if (Session.CurrentUser == null) return;

            _addresses = _service.GetAddresses(Session.CurrentUser.Id);
            AddressList.ItemsSource = _addresses;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser == null) return;

            var address = NewAddressTextBox.Text?.Trim();
            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Введите адрес");
                return;
            }

            _service.AddAddress(Session.CurrentUser.Id, address);
            NewAddressTextBox.Clear();
            LoadAddresses();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (AddressList.SelectedItem is not DeliveryAddressEntity address)
                return;

            _service.RemoveAddress(address.Id);
            LoadAddresses();
        }
    }
}
