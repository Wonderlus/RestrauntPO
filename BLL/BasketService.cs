using DAL;
using DAL.Entities;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BLL
{
    public class BasketService
    {
        public void AddDish(int customerId, int dishId)
        {
            using var context = new RestrauntContext();

            var item = context.Basket
                .FirstOrDefault(b => b.CustomerId == customerId && b.DishId == dishId);

            if (item != null)
                item.Quantity += 1;
            else
                context.Basket.Add(new BasketEntity
                {
                    CustomerId = customerId,
                    DishId = dishId,
                    Quantity = 1,
                    AddedAt = DateTime.UtcNow
                });

            context.SaveChanges();
        }


        public List<BasketItemModel> GetBasket(int customerId)
        {
            using var context = new RestrauntContext();

            return context.Basket
                .Where(b => b.CustomerId == customerId)
                .Join(context.Dishes,
                      b => b.DishId,
                      d => d.Id,
                      (b, d) => new BasketItemModel
                      {
                          DishId = d.Id,
                          Name = d.Name,
                          Price = d.Price,
                          Quantity = b.Quantity
                      })
                .ToList();
        }

        public void Remove(int customerId, int dishId)
        {
            using var context = new RestrauntContext();

            var item = context.Basket
                .FirstOrDefault(b => b.CustomerId == customerId && b.DishId == dishId);

            if (item != null)
            {
                context.Basket.Remove(item);
                context.SaveChanges();
            }
        }

        public void Increase(int customerId, int dishId)
        {
            using var context = new RestrauntContext();

            var item = context.Basket
                .FirstOrDefault(b => b.CustomerId == customerId && b.DishId == dishId);

            if (item != null)
            {
                item.Quantity++;
                context.SaveChanges();
            }
        }

        public void Decrease(int customerId, int dishId)
        {
            using var context = new RestrauntContext();

            var item = context.Basket
                .FirstOrDefault(b => b.CustomerId == customerId && b.DishId == dishId);

            if (item == null) return;

            item.Quantity--;

            if (item.Quantity <= 0)
                context.Basket.Remove(item);

            context.SaveChanges();
        }
    }
}
