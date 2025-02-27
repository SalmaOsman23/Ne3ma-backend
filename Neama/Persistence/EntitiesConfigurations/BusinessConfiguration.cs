using Ne3ma.Entities;

namespace Ne3ma.Persistence.EntitiesConfigurations;

public class BusinessConfiguration : IEntityTypeConfiguration<Business>
{
    public void Configure(EntityTypeBuilder<Business> builder)
    {
        builder.Property(b => b.Location)
            .HasColumnType("geography")
            .IsRequired();
    }
}
