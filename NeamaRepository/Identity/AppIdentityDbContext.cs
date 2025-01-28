using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neama.Core.Entities;
using Neama.Core.Entities.Identity;

namespace Neama.Repository.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SurpriseBag> SurpriseBags { get; set; }
        public DbSet<Order> Orders { get; set; }

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Supplier-SurpriseBag relationship
            builder.Entity<Supplier>()
             .HasMany(s => s.SurpriseBags)
             .WithOne(sb => sb.Supplier)
             .HasForeignKey(sb => sb.SupplierId)
             .OnDelete(DeleteBehavior.Cascade); // When a Supplier is deleted >> delete all its SurpriseBags


            // Configure SurpriseBag properties
            builder.Entity<SurpriseBag>()
            .Property(sb => sb.Price)
            .HasPrecision(18, 2); // 18  digits & 2 decimal places
        }
    }
}
