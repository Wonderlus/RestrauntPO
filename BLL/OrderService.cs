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

        
        public int CreateOrder(
            int customerId,
            string orderType,
            int? deliveryAddressId,
            TimeSpan? deliveryEta,          // ✅ TimeSpan?
            string? specialRequests)
        {
            using var context = new RestrauntContext();

            var basketItems = context.Basket
                .Include(b => b.Dish)
                .Where(b => b.CustomerId == customerId)
                .ToList();

            if (!basketItems.Any())
                throw new Exception("Корзина пуста");

            decimal totalAmount = basketItems.Sum(b => b.Quantity * b.Dish.Price);

            var order = new OrderEntity
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                OrderType = orderType,               // 'на месте' | 'самовывоз' | 'доставка'
                Status = "принят",
                DeliveryAddressId = orderType == "доставка" ? deliveryAddressId : null,
                DeliveryEta = orderType == "доставка" ? deliveryEta : null,  // ✅
                SpecialRequests = specialRequests,
                TotalAmount = totalAmount
            };

            context.Orders.Add(order);
            context.SaveChanges();

            foreach (var item in basketItems)
            {
                context.OrderItems.Add(new OrderItemEntity
                {
                    OrderId = order.Id,
                    DishId = item.DishId,
                    Quantity = item.Quantity,
                    PriceAtOrder = item.Dish.Price
                    // total computed — не трогаем
                });
            }

            context.Basket.RemoveRange(basketItems);
            context.SaveChanges();

            return order.Id;
        }
    }
}
