using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using InventorySystem.Core.Models;

namespace InventorySystem.Core.Data
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Activities> Activities { get; set; }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Ignore(p => p.Status);

            modelBuilder.Entity<Activities>()
                .HasKey(a => a.ActivityId);

            base.OnModelCreating(modelBuilder);

        }
    }
}
