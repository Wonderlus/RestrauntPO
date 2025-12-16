using DAL;
using Restraunt.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL
{
    public class DishService
    {
        /// <summary>
        /// Получить все блюда для меню
        /// (только видимые и "в наличии")
        /// </summary>
        public List<DishModel> GetMenuDishes()
        {
            using var context = new RestrauntContext();

            return context.Dishes
                .Include(d => d.Category)
                .Where(d =>
                    d.IsVisible &&
                    d.Status == "в наличии"
                )
                .Select(d => new DishModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CategoryName = d.Category != null ? d.Category.Name : null,
                    Price = d.Price,
                    ImageUrl = d.ImageUrl,
                    IsSeasonal = d.IsSeasonal,
                    IsPromotional = d.IsPromotional
                })
                .ToList();
        }
    }
}
