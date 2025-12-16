using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Restraunt.Models;

namespace BLL
{
    public class DishService
    {
        /// <summary>
        /// Получить блюда для меню (видимые и в наличии)
        /// </summary>
        public List<DishModel> GetMenuDishes()
        {
            using var context = new RestrauntContext();

            return context.Dishes
                .Include(d => d.Category)
                .Where(d => d.IsVisible && d.Status == "в наличии")
                .Select(d => ToModel(d))
                .ToList();
        }

        /// <summary>
        /// Получить блюдо по Id
        /// </summary>
        public DishModel? GetById(int id)
        {
            using var context = new RestrauntContext();

            var entity = context.Dishes
                .Include(d => d.Category)
                .FirstOrDefault(d => d.Id == id);

            return entity == null ? null : ToModel(entity);
        }

        /// <summary>
        /// Добавить новое блюдо
        /// </summary>
        public void Add(DishModel model)
        {
            using var context = new RestrauntContext();

            var entity = new DishEntity
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                IsVisible = true,
                IsSeasonal = model.IsSeasonal,
                IsPromotional = model.IsPromotional,
                Status = "в наличии"
            };

            context.Dishes.Add(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Обновить блюдо
        /// </summary>
        public void Update(DishModel model)
        {
            using var context = new RestrauntContext();

            var entity = context.Dishes.FirstOrDefault(d => d.Id == model.Id);
            if (entity == null) return;

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.CategoryId = model.CategoryId;
            entity.Price = model.Price;
            entity.ImageUrl = model.ImageUrl;
            entity.IsSeasonal = model.IsSeasonal;
            entity.IsPromotional = model.IsPromotional;

            context.SaveChanges();
        }

        /// <summary>
        /// Мягкое удаление (скрываем блюдо)
        /// </summary>
        public void Delete(int id)
        {
            using var context = new RestrauntContext();

            var entity = context.Dishes.FirstOrDefault(d => d.Id == id);
            if (entity == null) return;

            entity.IsVisible = false;
            context.SaveChanges();
        }

        /// <summary>
        /// Маппинг Entity → Model
        /// </summary>
        private static DishModel ToModel(DishEntity d)
        {
            return new DishModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CategoryName = d.Category?.Name,
                CategoryId = d.CategoryId,
                Price = d.Price,
                ImageUrl = d.ImageUrl,
                IsSeasonal = d.IsSeasonal,
                IsPromotional = d.IsPromotional
            };
        }
    }
}
