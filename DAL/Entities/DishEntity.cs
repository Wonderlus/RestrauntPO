using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    [Table("dishes")]
    public class DishEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("category_id")]
        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public DishCategoryEntity? Category { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("is_visible")]
        public bool IsVisible { get; set; }
        [Column("is_seasonal")]
        public bool IsSeasonal { get; set; }

        [Column("is_promotional")]
        public bool IsPromotional { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }
    }
}
