using Ne3ma.Entities;

namespace Ne3ma.Persistence.EntitiesConfigurations;

public class FoodItemConfiguration : IEntityTypeConfiguration<FoodItem>
{
    public void Configure(EntityTypeBuilder<FoodItem> builder)
    {
        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");
    }
}