using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RestrauntContext : DbContext
    {
        public DbSet<DishEntity> Dishes { get; set; }
        public DbSet<DishCategoryEntity> DishCategories { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<DeliveryAddressEntity> DeliveryAddresses { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<BasketEntity> Basket { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=RestrauntPO;Username=postgres;Password=zxc123"
            );
        }
    }
}
