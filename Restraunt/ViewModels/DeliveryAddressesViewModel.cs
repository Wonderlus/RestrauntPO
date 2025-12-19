using BLL;
using CommunityToolkit.Mvvm.Input;
using DAL.Entities;
using Restraunt.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class DeliveryAddressesViewModel : ViewModelBase
    {
        private readonly DeliveryAddressService _service = new();

        public ObservableCollection<DeliveryAddressEntity> Addresses { get; } = new();

        private DeliveryAddressEntity? _selectedAddress;
        public DeliveryAddressEntity? SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                _selectedAddress = value;
                OnPropertyChanged();
                (RemoveCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        private string _newAddress = "";
        public string NewAddress
        {
            get => _newAddress;
            set
            {
                _newAddress = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }

        public DeliveryAddressesViewModel()
        {
            AddCommand = new RelayCommand(OnAdd);
            RemoveCommand = new RelayCommand(OnRemove, CanRemove);

            LoadAddresses();
        }

        private void LoadAddresses()
        {
            if (Session.CurrentUser == null) return;

            Addresses.Clear();
            var addresses = _service.GetAddresses(Session.CurrentUser.Id);
            foreach (var addr in addresses)
            {
                Addresses.Add(addr);
            }
        }

        private void OnAdd()
        {
            if (Session.CurrentUser == null) return;

            var address = NewAddress?.Trim();
            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Введите адрес");
                return;
            }

            _service.AddAddress(Session.CurrentUser.Id, address);
            NewAddress = "";
            LoadAddresses();
        }

        private bool CanRemove() => SelectedAddress != null;

        private void OnRemove()
        {
            if (SelectedAddress == null) return;

            _service.RemoveAddress(SelectedAddress.Id);
            LoadAddresses();
        }
    }
}
