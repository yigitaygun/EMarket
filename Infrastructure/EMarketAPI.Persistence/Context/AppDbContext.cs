    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using EMarketAPI.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using System.IO;

namespace EMarketAPI.Persistence.Context
    {
        public class AppDbContext : DbContext
        {

            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<AppUser> Users => Set<AppUser>();
            public DbSet<Product> Products => Set<Product>();
            public DbSet<Order> Orders => Set<Order>();
            public DbSet<OrderItem> OrderItems => Set<OrderItem>();


            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Order>()
                    .HasMany(o => o.Items)
                    .WithOne(i => i.Order)
                    .HasForeignKey(i => i.OrderId);

                modelBuilder.Entity<Order>()
                    .HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId);

                modelBuilder.Entity<OrderItem>()
                    .HasOne(i => i.Product)
                    .WithMany()
                    .HasForeignKey(i => i.ProductId);
        }   


        }

 
    }

