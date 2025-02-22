namespace Ne3ma.Entities;

public class FoodItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int QuantityAvailable { get; set; }
    public DateTime ExpiryTime { get; set; }
    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = [];
    public bool IsAvailable => QuantityAvailable > 0 && ExpiryTime > DateTime.UtcNow;
}
