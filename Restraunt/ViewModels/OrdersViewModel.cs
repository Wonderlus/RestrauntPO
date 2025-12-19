using BLL;
using CommunityToolkit.Mvvm.Input;
using DAL.Entities;
using Restraunt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private readonly OrderService _orderService = new();

        // ===== РОЛЬ =====
        public bool IsAdmin => Session.CurrentUser?.IsAdmin == true;

        // ===== ВЫЧИСЛЯЕМЫЕ СУММЫ =====

        /// <summary>
        /// Сумма заказа БЕЗ скидки
        /// </summary>
        public decimal Subtotal =>
            SelectedOrderItems?.Sum(i => i.PriceAtOrder * i.Quantity) ?? 0;

        /// <summary>
        /// Процент скидки (0–40)
        /// </summary>
        public decimal DiscountPercent =>
            SelectedOrder == null ? 0 : (1 - SelectedOrder.Discount) * 100;

        /// <summary>
        /// Сумма скидки в рублях
        /// </summary>
        public decimal DiscountAmount =>
            SelectedOrder == null ? 0 : Subtotal * (1 - SelectedOrder.Discount);

        // ===== ФИЛЬТРЫ =====
        public List<string> Statuses { get; } = new()
        {
            "Все",
            "принят",
            "готовится",
            "готов",
            "доставляется",
            "выполнен",
            "отменен"
        };

        public List<string> OrderTypes { get; } = new()
        {
            "Все",
            "на месте",
            "самовывоз",
            "доставка"
        };

        private string _selectedStatus = "Все";
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private string _selectedOrderType = "Все";
        public string SelectedOrderType
        {
            get => _selectedOrderType;
            set
            {
                _selectedOrderType = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        // ===== ДАННЫЕ =====
        private List<OrderEntity> _allOrders = new();

        private List<OrderEntity> _orders = new();
        public List<OrderEntity> Orders
        {
            get => _orders;
            private set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        private OrderEntity? _selectedOrder;
        public OrderEntity? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedOrder));
                OnPropertyChanged(nameof(CanCancel));
                OnPropertyChanged(nameof(CanEdit));

                LoadOrderItems();

                // обновляем расчёты ПОСЛЕ загрузки позиций
                OnPropertyChanged(nameof(Subtotal));
                OnPropertyChanged(nameof(DiscountPercent));
                OnPropertyChanged(nameof(DiscountAmount));

                // Notify commands that CanExecute may have changed
                (EditOrderCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (CancelOrderCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool HasSelectedOrder => SelectedOrder != null;

        public List<OrderItemEntity> SelectedOrderItems { get; private set; } = new();

        // ===== ПРАВА НА ДЕЙСТВИЯ =====
        public bool CanCancel =>
            SelectedOrder != null &&
            (IsAdmin ||
             (SelectedOrder.CustomerId == Session.CurrentUser!.Id &&
              (SelectedOrder.Status == "принят" || SelectedOrder.Status == "готовится")));

        public bool CanEdit =>
            SelectedOrder != null &&
            (IsAdmin ||
             (SelectedOrder.CustomerId == Session.CurrentUser!.Id &&
              SelectedOrder.Status == "принят"));


        public DateTime ExportFrom { get; set; } = DateTime.Today.AddDays(-7);
        public DateTime ExportTo { get; set; } = DateTime.Today;

        // ===== COMMANDS =====
        public ICommand ExportOrdersCommand { get; }
        public ICommand EditOrderCommand { get; }
        public ICommand CancelOrderCommand { get; }
        public ICommand ResetFiltersCommand { get; }

        // ===== EVENTS FOR DIALOG REQUESTS =====
        public event Action<OrderEntity>? RequestEditOrder;

        // ===== КОНСТРУКТОР =====
        public OrdersViewModel()
        {
            ExportOrdersCommand = new RelayCommand(ExportOrdersToExcel);
            EditOrderCommand = new RelayCommand(OnEditOrder, () => CanEdit);
            CancelOrderCommand = new RelayCommand(OnCancelOrder, () => CanCancel);
            ResetFiltersCommand = new RelayCommand(ResetFilters);

            LoadOrders();
        }

        // ===== ЗАГРУЗКА =====
        public void LoadOrders()
        {
            if (Session.CurrentUser == null) return;

            _allOrders = _orderService.GetOrders(
                Session.CurrentUser.Id,
                IsAdmin);

            ApplyFilters();
        }

        private void LoadOrderItems()
        {
            if (SelectedOrder == null)
            {
                SelectedOrderItems = new();
            }
            else
            {
                SelectedOrderItems = _orderService.GetOrderItems(SelectedOrder.Id);
            }

            OnPropertyChanged(nameof(SelectedOrderItems));
        }

        // ===== ФИЛЬТРАЦИЯ =====
        private void ApplyFilters()
        {
            IEnumerable<OrderEntity> query = _allOrders;

            if (SelectedStatus != "Все")
                query = query.Where(o => o.Status == SelectedStatus);

            if (SelectedOrderType != "Все")
                query = query.Where(o => o.OrderType == SelectedOrderType);

            Orders = query
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        private void ResetFilters()
        {
            SelectedStatus = "Все";
            SelectedOrderType = "Все";
        }

        // ===== ДЕЙСТВИЯ =====
        private void OnEditOrder()
        {
            if (SelectedOrder == null) return;
            RequestEditOrder?.Invoke(SelectedOrder);
        }

        private void OnCancelOrder()
        {
            if (SelectedOrder == null) return;

            var result = MessageBox.Show(
                "Вы действительно хотите отменить заказ?",
                "Отмена заказа",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
                CancelSelectedOrder();
        }

        public void CancelSelectedOrder()
        {
            if (SelectedOrder == null || Session.CurrentUser == null)
                return;

            _orderService.CancelOrder(
                SelectedOrder.Id,
                Session.CurrentUser.Id,
                IsAdmin);

            LoadOrders();
            SelectedOrder = null;
        }

        public void ExportOrdersToExcel()
        {
            if (!IsAdmin)
                return;

            var orders = _orderService.GetOrdersForPeriod(
                ExportFrom,
                ExportTo.AddDays(1).AddSeconds(-1)
            );

            ExcelExportService.ExportOrders(orders);
        }


        public void Reload()
        {
            LoadOrders();
        }
    }
}
