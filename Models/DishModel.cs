using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restraunt.Models
{
    public class DishModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string CategoryName { get; set; }

        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public bool IsSeasonal { get; set; }
        public bool IsPromotional { get; set; }
    }
}
