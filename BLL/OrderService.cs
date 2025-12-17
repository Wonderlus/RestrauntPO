using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;



namespace BLL
{
    public class OrderService
    {


        public decimal CreateOrder(
    int customerId,
    string orderType,
    int? deliveryAddressId,
    TimeSpan? deliveryEta,
    string? specialRequests)
        {
            using var context = new RestrauntContext();

            var basketItems = context.Basket
                .Where(b => b.CustomerId == customerId)
                .Include(b => b.Dish)
                .ToList();

            if (!basketItems.Any())
                throw new Exception("Корзина пуста");

            // 1. Считаем сумму БЕЗ скидки
            decimal subtotal = basketItems.Sum(b => b.Dish.Price * b.Quantity);

            // 2. Считаем коэффициент скидки
            decimal discount = CalculateDiscount(subtotal);

            // 3. Итоговая сумма
            decimal totalWithDiscount = subtotal * discount;


            // 3. Создаём заказ
            var order = new OrderEntity
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                OrderType = orderType,
                Status = "принят",
                DeliveryAddressId = deliveryAddressId,
                DeliveryEta = deliveryEta,
                SpecialRequests = specialRequests,
                TotalAmount = totalWithDiscount,
                Discount = discount
                
            };

            context.Orders.Add(order);
            context.SaveChanges();

            // 4. Переносим позиции корзины в order_items
            foreach (var item in basketItems)
            {
                context.OrderItems.Add(new OrderItemEntity
                {
                    OrderId = order.Id,
                    DishId = item.DishId,
                    Quantity = item.Quantity,
                    PriceAtOrder = item.Dish.Price
                    // total НЕ трогаем — computed column
                });
            }

            // 5. Очищаем корзину
            context.Basket.RemoveRange(basketItems);
            Console.WriteLine($"Discount before save: {discount}");
            context.SaveChanges();
            return discount;
        }


        public List<OrderEntity> GetOrdersByCustomer(int customerId)
        {
            using var context = new RestrauntContext();

            return context.Orders
    .Include(o => o.DeliveryAddress)
    .Where(o => o.CustomerId == customerId)
    .OrderByDescending(o => o.OrderDate)
    .ToList();
        }


        public List<OrderItemEntity> GetOrderItems(int orderId)
        {
            using var context = new RestrauntContext();

            return context.OrderItems
                .Include(oi => oi.Dish)
                .Where(oi => oi.OrderId == orderId)
                .ToList();
        }


        public void CancelOrder(int orderId, int currentUserId, bool isAdmin)
        {
            using var context = new RestrauntContext();

            var order = context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                throw new Exception("Заказ не найден");

            // защита
            if (!isAdmin && order.CustomerId != currentUserId)
                throw new Exception("Нет доступа к заказу");

            if (!isAdmin && order.Status != "принят" && order.Status != "готовится")
                throw new Exception("Этот заказ нельзя отменить");

            order.Status = "отменен";
            context.SaveChanges();
        }

        public void UpdateOrder(
    int orderId,
    int currentUserId,
    bool isAdmin,
    string orderType,
    int? deliveryAddressId,
    TimeSpan? deliveryEta,
    string? specialRequests)
        {
            using var context = new RestrauntContext();

            var order = context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                throw new Exception("Заказ не найден");

            // доступ
            if (!isAdmin && order.CustomerId != currentUserId)
                throw new Exception("Нет доступа к заказу");

            // редактировать можно только "принят" (для пользователя), админ — всегда
            if (!isAdmin && order.Status != "принят")
                throw new Exception("Редактировать можно только принятый заказ");

            // если не доставка — адрес/время чистим
            order.OrderType = orderType;

            if (orderType != "доставка")
            {
                order.DeliveryAddressId = null;
                order.DeliveryEta = null;
            }
            else
            {
                order.DeliveryAddressId = deliveryAddressId;
                order.DeliveryEta = deliveryEta;
            }

            order.SpecialRequests = specialRequests;
            context.SaveChanges();
        }

        public void SetOrderStatus(int orderId, string newStatus, int currentUserId, bool isAdmin)
        {
            if (!isAdmin)
                throw new Exception("Недостаточно прав");

            using var context = new RestrauntContext();

            var order = context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                throw new Exception("Заказ не найден");

            order.Status = newStatus;
            context.SaveChanges();
        }

        public List<OrderEntity> GetOrders(int currentUserId, bool isAdmin)
        {
            using var context = new RestrauntContext();

            var query = context.Orders
                .Include(o => o.DeliveryAddress)
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(o => o.CustomerId == currentUserId);

            return query.ToList();
        }

        

        private decimal CalculateDiscount(decimal total)
        {
            // За каждую 1000 ₽ — 10%, максимум 40%
            int thousands = (int)(total / 1000);
            int discountPercent = thousands * 10;

            if (discountPercent > 40)
                discountPercent = 40;

            decimal discountMultiplier = 1m - (discountPercent / 100m);

            return discountMultiplier;
        }


        public List<OrderEntity> GetOrdersForPeriod(
    DateTime dateFrom,
    DateTime dateTo)
        {
            using var context = new RestrauntContext();

            var fromUtc = DateTime.SpecifyKind(dateFrom, DateTimeKind.Utc);
            var toUtc = DateTime.SpecifyKind(dateTo, DateTimeKind.Utc);

            return context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DeliveryAddress)
                .Where(o => o.OrderDate >= fromUtc && o.OrderDate <= toUtc)
                .OrderBy(o => o.OrderDate)
                .ToList();
        }


    }
}
