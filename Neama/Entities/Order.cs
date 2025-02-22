namespace Ne3ma.Entities;

public class Order : AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FoodItemId { get; set; }
    public FoodItem FoodItem { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime? PickupTime { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalPrice => Quantity * FoodItem.Price;
}
