using BLL;
using CommunityToolkit.Mvvm.Input;
using DAL.Entities;
using Restraunt.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class EditOrderViewModel : ViewModelBase
    {
        private readonly OrderService _orderService = new();
        private readonly DeliveryAddressService _addressService = new();

        public OrderEntity Order { get; }

        public bool IsAdmin => Session.CurrentUser?.IsAdmin == true;
        public bool IsDelivery => SelectedOrderType == "доставка";

        public List<string> Statuses { get; } = new()
        {
            "принят",
            "готовится",
            "готов",
            "доставляется",
            "выполнен",
            "отменен"
        };

        public List<string> OrderTypes { get; } = new()
        {
            "на месте",
            "самовывоз",
            "доставка"
        };

        private string _selectedOrderType;
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

        public List<DeliveryAddressEntity> DeliveryAddresses { get; private set; } = new();

        private DeliveryAddressEntity? _selectedAddress;
        public DeliveryAddressEntity? SelectedAddress
        {
            get => _selectedAddress;
            set { _selectedAddress = value; OnPropertyChanged(); }
        }

        // строка времени "HH:mm"
        private string _deliveryTimeText = "01:00";
        public string DeliveryTimeText
        {
            get => _deliveryTimeText;
            set { _deliveryTimeText = value; OnPropertyChanged(); }
        }

        private string? _specialRequests;
        public string? SpecialRequests
        {
            get => _specialRequests;
            set { _specialRequests = value; OnPropertyChanged(); }
        }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set { _selectedStatus = value; OnPropertyChanged(); }
        }

        public bool CanEditType => IsAdmin || Order.Status == "принят";

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Action callback for closing the window with result
        public Action<bool>? RequestClose { get; set; }

        public EditOrderViewModel(OrderEntity order)
        {
            Order = order;
            SelectedOrderType = order.OrderType;
            // загружаем адреса того пользователя, которому принадлежит заказ
            DeliveryAddresses = _addressService.GetAddresses(order.CustomerId);
            OnPropertyChanged(nameof(DeliveryAddresses));

            // выставляем текущие значения
            SpecialRequests = order.SpecialRequests;

            if (order.DeliveryEta.HasValue)
                DeliveryTimeText = order.DeliveryEta.Value.ToString(@"hh\:mm");

            // пытаемся выбрать текущий адрес
            if (order.DeliveryAddressId.HasValue)
                SelectedAddress = DeliveryAddresses.FirstOrDefault(a => a.Id == order.DeliveryAddressId.Value);

            SelectedStatus = order.Status;

            // Initialize commands
            SaveCommand = new RelayCommand(OnSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private void OnSave()
        {
            if (Save())
            {
                RequestClose?.Invoke(true);
            }
        }

        private void OnCancel()
        {
            RequestClose?.Invoke(false);
        }

        public bool Save()
        {
            if (Session.CurrentUser == null)
                return false;

            TimeSpan? eta = null;

            if (IsDelivery)
            {
                if (SelectedAddress == null)
                {
                    MessageBox.Show("Выберите адрес доставки");
                    return false;
                }

                if (!TimeSpan.TryParseExact(DeliveryTimeText, @"hh\:mm", CultureInfo.InvariantCulture, out var parsed))
                {
                    MessageBox.Show("Неверный формат времени. Используй HH:mm, например 01:30");
                    return false;
                }

                eta = parsed;
            }

            try
            {
                // 1) редактируем заказ (адрес/время/коммент)
                _orderService.UpdateOrder(
                    Order.Id,
                    Session.CurrentUser.Id,
                    IsAdmin,
                    SelectedOrderType,
                    IsDelivery ? SelectedAddress?.Id : null,
                    IsDelivery ? eta : null,
                    SpecialRequests
                );

                // 2) статус — только для админа
                if (IsAdmin && SelectedStatus != Order.Status)
                {
                    _orderService.SetOrderStatus(
                        Order.Id,
                        SelectedStatus,
                        Session.CurrentUser.Id,
                        true
                    );
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
