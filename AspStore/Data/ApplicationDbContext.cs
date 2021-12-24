using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AspStore.Models;

namespace AspStore.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().Property(m=>m.SalesPrice).HasDefaultValue(0);
            builder.Entity<Product>().Property(m=>m.RegularPrice).HasDefaultValue(0);
            builder.Entity<Cart>().Property(m=>m.Quantity).HasDefaultValue(1);
            builder.Entity<Cart>().HasOne<Product>(m => m.Product)
                .WithMany(m => m.Cart).HasForeignKey(m => m.ProductId)
                .IsRequired(true).OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
