using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace DAL.Entities
{
    [Table("customers")]
    public class CustomerEntity
    {
        [Column("id")]
        public int Id { get; set; }


        [Column("full_name")]
        public string FullName { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("registration_date")]
        public DateTime RegistrationDate { get; set; }

        [Column("is_admin")]
        public bool IsAdmin { get; set; }

        [Column("loyalty_points")]
        public int LoyaltyPoints { get; set; } = 0;
    }
}
