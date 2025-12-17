using BLL;
using DAL.Entities;
using Restraunt.Services;
using System.Collections.Generic;

namespace Restraunt.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {

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

        private List<OrderEntity> _allOrders = new();

        private readonly OrderService _orderService = new();
        public bool HasSelectedOrder => SelectedOrder != null;
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
        public List<OrderItemEntity> SelectedOrderItems { get; private set; } = new();

        private OrderEntity? _selectedOrder;
        public OrderEntity? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedOrder));
                LoadOrderItems();
            }
        }

        public OrdersViewModel()
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            if (Session.CurrentUser == null) return;

            _allOrders = _orderService.GetOrdersByCustomer(Session.CurrentUser.Id);
            ApplyFilters();
        }


        private void LoadOrderItems()
        {
            if (SelectedOrder == null) return;
            SelectedOrderItems = _orderService.GetOrderItems(SelectedOrder.Id);
            OnPropertyChanged(nameof(SelectedOrderItems));
        }

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

    }
}
