using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{

    [Table("order_items")]
    public class OrderItemEntity
    {

        [Column("id")]
        public int Id { get; set; }


        [Column("order_id")]
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public OrderEntity Order { get; set; }

        [Column("dish_id")]

        public int DishId { get; set; }

        [ForeignKey(nameof(DishId))]
        public DishEntity Dish { get; set; }


        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("price_at_order")]
        public decimal PriceAtOrder { get; set; }


        [Column("total")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Total { get; set; }
    }
}
