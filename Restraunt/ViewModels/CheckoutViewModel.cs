using BLL;
using CommunityToolkit.Mvvm.Input;
using DAL.Entities;
using Restraunt.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {

        private readonly DeliveryAddressService _addressService = new();
        public DeliveryAddressEntity? SelectedAddress { get; set; }
        public List<DeliveryAddressEntity> DeliveryAddresses { get; private set; } = new();
        private readonly OrderService _orderService = new();

        public CheckoutViewModel(decimal totalAmount)
        {

            DeliveryAddresses = _addressService.GetAddresses(Session.CurrentUser.Id);
            OnPropertyChanged(nameof(DeliveryAddresses));
            TotalAmount = totalAmount;

            OrderTypes = new()
            {
                "на месте",
                "самовывоз",
                "доставка"
            };

            SelectedOrderType = "доставка";
            DeliveryTimeText = "01:00";

            ConfirmOrderCommand = new RelayCommand(CreateOrder);
        }

        public List<string> OrderTypes { get; }

        private string _selectedOrderType = "доставка";
        public string SelectedOrderType
        {
            get => _selectedOrderType;
            set
            {
                _selectedOrderType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDelivery));
            }
        }

        public bool IsDelivery => SelectedOrderType == "доставка";


        // ✅ вместо DateTime?
        public string DeliveryTimeText { get; set; } // "HH:mm"

        public string? SpecialRequests { get; set; }
        public decimal TotalAmount { get; }

        public ICommand ConfirmOrderCommand { get; }
        public event Action? OrderCreated;

        private void CreateOrder()
        {
            TimeSpan? eta = null;

            if (IsDelivery)
            {
                if (!TimeSpan.TryParseExact(DeliveryTimeText, @"hh\:mm", CultureInfo.InvariantCulture, out var parsed))
                    throw new Exception("Неверный формат времени. Используй HH:mm, например 01:30");

                eta = parsed;
            }

            _orderService.CreateOrder(
                Session.CurrentUser.Id,
                SelectedOrderType,
                SelectedAddress?.Id,
                eta,
                SpecialRequests
            );

            OrderCreated?.Invoke();
        }
    }

}
