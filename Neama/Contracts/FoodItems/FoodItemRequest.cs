namespace Ne3ma.Contracts.FoodItems;

public record FoodItemRequest(
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    int QuantityAvailable,
    DateTime ExpiryTime
);