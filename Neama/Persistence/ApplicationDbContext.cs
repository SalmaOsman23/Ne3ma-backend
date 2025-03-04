using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ne3ma.Entities;
using Ne3ma.Persistence.EntitiesConfigurations;
using Neama.Extentions;
using System.Reflection;

namespace Neama.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<IdentityUser>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


    public DbSet<Business> Businesses { get; set; } = default!;
    public DbSet<FoodItem> FoodItems { get; set; } = default!;


    //public DbSet<Order> Orders { get; set; } = default!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeFKs = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        modelBuilder.Entity<FoodItem>()
            .ToTable("FoodItems");

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();

        foreach (var entityEntry in entries)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()!;
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;

            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
