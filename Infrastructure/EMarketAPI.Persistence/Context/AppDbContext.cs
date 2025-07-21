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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EMarketAPI.Persistence.Identity;

namespace EMarketAPI.Persistence.Context
    {
        public class AppDbContext :IdentityDbContext<AppUser>
        {

            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
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

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId);
        }



    }

 
    }

