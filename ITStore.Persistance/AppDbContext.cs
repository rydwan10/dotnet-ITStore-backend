using Microsoft.EntityFrameworkCore;
using System;
using ITStore.Domain;
using ITStore.Shared;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ITStore.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUsers, IdentityRole<Guid>, Guid>
    {
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Discounts> Discounts { get; set; } 
        public virtual DbSet<Inventories> Inventories { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<UserAddresses> UserAddresses { get; set; }
        public virtual DbSet<Orders> Orders { get; set;  }
        public virtual DbSet<OrderPayments> OrderPayments { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set;  }
        public virtual DbSet<ShippingAddresses> ShippingAddresses { get; set; }
        public virtual DbSet<Wishlists> Wishlists { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }

        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; Database=ITStoreDev2; Username=postgres; Password=admin");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Filter Soft Delete
            modelBuilder.Entity<Categories>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);
            modelBuilder.Entity<Discounts>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);
            modelBuilder.Entity<Inventories>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);
            modelBuilder.Entity<Products>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);
            modelBuilder.Entity<UserAddresses>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);
            modelBuilder.Entity<Carts>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);
            modelBuilder.Entity<Wishlists>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);
            modelBuilder.Entity<Orders>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);
            modelBuilder.Entity<OrderPayments>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);
            modelBuilder.Entity<OrderItems>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);
            modelBuilder.Entity<ShippingAddresses>().HasQueryFilter(x => x.StatusRecord != Constants.StatusRecordDelete);

            // Restrict Delete Related Data (Foreign Key)
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
