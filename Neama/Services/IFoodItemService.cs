using Ne3ma.Contracts.FoodItems;

namespace Ne3ma.Services;

public interface IFoodItemService
{
    Task<Result<ICollection<FoodItemResponse>>> GetAllAsync();
    Task<Result<FoodItemResponse>> GetByIdAsync(Guid id);
    Task<Result<FoodItemResponse>> CreateAsync(FoodItemRequest request);
    Task<Result> UpdateAsync(Guid id, FoodItemRequest request);
    Task<Result> DeleteAsync(Guid id);
}
