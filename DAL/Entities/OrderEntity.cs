using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{

    [Table("orders")]
    public class OrderEntity
    {

        [Column("id")]
        public int Id { get; set; }


        [Column("customer_id")]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]

        public CustomerEntity Customer { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        [Column("order_type")]
        public string OrderType { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("delivery_address_id")]

        public int? DeliveryAddressId { get; set; }

        [ForeignKey(nameof(DeliveryAddressId))]
        public DeliveryAddressEntity? DeliveryAddress { get; set; }

        [Column("delivery_eta")]

        public TimeSpan? DeliveryEta { get; set; }


        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("special_requests")]
        public string? SpecialRequests { get; set; }
        [Column("discount")]
        public decimal Discount { get; set; } // ← НОВОЕ ПОЛЕ
    }
}
