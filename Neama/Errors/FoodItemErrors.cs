namespace Ne3ma.Errors;

public static class FoodItemErrors
{
    public static readonly Error NotFound =
        new("FoodItem.NotFound", "The requested food item was not found.", StatusCodes.Status404NotFound);

}
