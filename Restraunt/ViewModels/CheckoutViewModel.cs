using BLL;
using CommunityToolkit.Mvvm.Input;
using DAL.Entities;
using Restraunt.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
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
            CancelCommand = new RelayCommand(OnCancel);
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

        public string DeliveryTimeText { get; set; } // "HH:mm"

        public string? SpecialRequests { get; set; }
        public decimal TotalAmount { get; }

        public ICommand ConfirmOrderCommand { get; }
        public ICommand CancelCommand { get; }
        
        public event Action? OrderCreated;
        public event Action? Cancelled;

        private void OnCancel()
        {
            Cancelled?.Invoke();
        }

        private void CreateOrder()
        {
            // Проверка адреса при доставке
            if (IsDelivery && SelectedAddress == null)
            {
                MessageBox.Show(
                    "Пожалуйста, выберите адрес доставки",
                    "Оформление заказа",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            TimeSpan? eta = null;

            if (IsDelivery)
            {
                if (!TimeSpan.TryParseExact(
                        DeliveryTimeText,
                        @"hh\:mm",
                        CultureInfo.InvariantCulture,
                        out var parsed))
                {
                    MessageBox.Show(
                        "Неверный формат времени.\nИспользуйте HH:mm, например 01:30",
                        "Оформление заказа",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                eta = parsed;
            }
            
            var discount = _orderService.CreateOrder(
                Session.CurrentUser.Id,
                SelectedOrderType,
                SelectedAddress?.Id,
                eta,
                SpecialRequests
            );

            if (discount < 1)
            {
                MessageBox.Show(
                    $"Вам применена скидка {(int)((1 - discount) * 100)}%",
                    "Скидка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            OrderCreated?.Invoke();
        }
    }
}
