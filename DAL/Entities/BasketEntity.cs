using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{

    [Table("basket")]
    public class BasketEntity
    {

        [Column("id")]
        public int Id { get; set; }


        [Column("customer_id")]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public CustomerEntity Customer { get; set; }


        [Column("dish_id")]
        public int DishId { get; set; }

        [ForeignKey(nameof(DishId))]
        public DishEntity Dish { get; set; }


        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("added_at")]
        public DateTime AddedAt { get; set; }
    }
}
