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
        private readonly LoyaltyService _loyaltyService = new();

        private decimal _subtotal; // Сумма корзины без скидок
        private int _pointsToUse = 0;

        public CheckoutViewModel(decimal subtotal)
        {
            _subtotal = subtotal;
            DeliveryAddresses = _addressService.GetAddresses(Session.CurrentUser.Id);
            OnPropertyChanged(nameof(DeliveryAddresses));

            // Загружаем доступные баллы
            AvailablePoints = _loyaltyService.GetCustomerPoints(Session.CurrentUser.Id);

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

            // Пересчитываем итоговую сумму
            RecalculateTotal();
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

        // Баллы
        public int AvailablePoints { get; private set; }

        public int PointsToUse
        {
            get => _pointsToUse;
            set
            {
                if (value < 0) value = 0;
                if (value > AvailablePoints) value = AvailablePoints;
                _pointsToUse = value;
                OnPropertyChanged();
                RecalculateTotal();
            }
        }

        public decimal TotalAmount { get; private set; }

        public decimal PointsDiscountAmount { get; private set; }

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

            try
            {
                var discount = _orderService.CreateOrder(
                    Session.CurrentUser.Id,
                    SelectedOrderType,
                    SelectedAddress?.Id,
                    eta,
                    SpecialRequests,
                    PointsToUse
                );

                // Обновляем баланс баллов в сессии
                if (Session.CurrentUser != null)
                {
                    Session.CurrentUser.LoyaltyPoints = _loyaltyService.GetCustomerPoints(Session.CurrentUser.Id);
                    Session.NotifyUserUpdated();
                }

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
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка при создании заказа",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void RecalculateTotal()
        {
            // 1. Применяем скидку от суммы заказа
            decimal discountMultiplier = OrderService.CalculateDiscount(_subtotal);
            decimal totalAfterDiscount = _subtotal * discountMultiplier;

            // 2. Применяем скидку от баллов
            PointsDiscountAmount = Math.Min(PointsToUse, totalAfterDiscount);
            TotalAmount = totalAfterDiscount - PointsDiscountAmount;

            OnPropertyChanged(nameof(TotalAmount));
            OnPropertyChanged(nameof(PointsDiscountAmount));
        }
    }
}
