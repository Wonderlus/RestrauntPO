using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    [Table("delivery_addresses")]
    public class DeliveryAddressEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("customer_id")]

        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public CustomerEntity Customer { get; set; }

        [Column("address")]

        public string Address { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [Column("created_at")]

        public DateTime CreatedAt { get; set; }
    }
}
