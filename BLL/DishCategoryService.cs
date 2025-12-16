using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DishCategoryService
    {
        public List<DishCategoryModel> GetAll()
        {
            using var context = new RestrauntContext();

            return context.DishCategories
                .Select(c => new DishCategoryModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }
    }
}
