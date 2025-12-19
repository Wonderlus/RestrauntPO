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
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Port=5432;Database=RestrauntPO;Username=postgres;Password=zxc123"
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка CustomerEntity
            modelBuilder.Entity<CustomerEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Phone).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.RegistrationDate).IsRequired();
            });

            // Настройка DishCategoryEntity
            modelBuilder.Entity<DishCategoryEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Настройка DishEntity
            modelBuilder.Entity<DishEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).IsRequired().HasPrecision(10, 2);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Настройка DeliveryAddressEntity
            modelBuilder.Entity<DeliveryAddressEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Comment).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Customer)
                    .WithMany()
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Настройка OrderEntity
            modelBuilder.Entity<OrderEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.OrderType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TotalAmount).IsRequired().HasPrecision(10, 2);
                entity.Property(e => e.SpecialRequests).HasMaxLength(1000);
                entity.Property(e => e.Discount).IsRequired().HasPrecision(5, 4).HasDefaultValue(1.0m);

                entity.HasOne(e => e.Customer)
                    .WithMany()
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DeliveryAddress)
                    .WithMany()
                    .HasForeignKey(e => e.DeliveryAddressId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.OrderDate);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CustomerId);
            });

            // Настройка OrderItemEntity
            modelBuilder.Entity<OrderItemEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.PriceAtOrder).IsRequired().HasPrecision(10, 2);
                entity.Property(e => e.Total).IsRequired().HasPrecision(10, 2);

                entity.HasOne(e => e.Order)
                    .WithMany()
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Dish)
                    .WithMany()
                    .HasForeignKey(e => e.DishId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.OrderId);
            });

            // Настройка BasketEntity
            modelBuilder.Entity<BasketEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.AddedAt).IsRequired();

                entity.HasOne(e => e.Customer)
                    .WithMany()
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Dish)
                    .WithMany()
                    .HasForeignKey(e => e.DishId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Уникальный индекс для предотвращения дубликатов
                entity.HasIndex(e => new { e.CustomerId, e.DishId }).IsUnique();
            });
        }

        /// <summary>
        /// Создает базу данных, если она не существует, и применяет все миграции
        /// </summary>
        public void EnsureDatabaseCreated()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Применяет все ожидающие миграции к базе данных
        /// </summary>
        public void MigrateDatabase()
        {
            Database.Migrate();
        }
    }
}
