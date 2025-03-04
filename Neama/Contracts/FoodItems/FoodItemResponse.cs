namespace Ne3ma.Contracts.FoodItems;

public record FoodItemResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    int QuantityAvailable,
    DateTime ExpiryTime,
    Guid BusinessId
);
