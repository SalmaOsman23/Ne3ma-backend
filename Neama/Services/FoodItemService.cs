using Ne3ma.Contracts.FoodItems;
using Ne3ma.Entities;
using Ne3ma.Errors;
using Neama.Extentions;

namespace Ne3ma.Services;

public class FoodItemService(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor) : IFoodItemService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<FoodItemResponse>> CreateAsync(FoodItemRequest request)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

        if (string.IsNullOrWhiteSpace(currentUserId))
            return Result.Failure<FoodItemResponse>(BusinessErrors.Unauthorized);

        var business = await _context.Businesses.FirstOrDefaultAsync(b => b.UserId == currentUserId);

        if (business is null)
            return Result.Failure<FoodItemResponse>(BusinessErrors.Forbidden);


        var foodItem = new FoodItem
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = request.ImageUrl,
            QuantityAvailable = request.QuantityAvailable,
            ExpiryTime = request.ExpiryTime,
            BusinessId = business.Id
        };

        await _context.FoodItems.AddAsync(foodItem);
        await _context.SaveChangesAsync();

        return Result.Success(new FoodItemResponse(
            foodItem.Id,
            foodItem.Name,
            foodItem.Description,
            foodItem.Price,
            foodItem.ImageUrl,
            foodItem.QuantityAvailable,
            foodItem.ExpiryTime,
            foodItem.BusinessId
        ));
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

        if (string.IsNullOrWhiteSpace(currentUserId))
            return Result.Failure(BusinessErrors.Unauthorized);

        var foodItem = await _context.FoodItems.Include(f => f.Business).FirstOrDefaultAsync(f => f.Id == id);
        if (foodItem is null)
            return Result.Failure(FoodItemErrors.NotFound);

        //  only the business owner can delete
        if (foodItem.Business.UserId != currentUserId)
            return Result.Failure(BusinessErrors.Forbidden);

        _context.FoodItems.Remove(foodItem);

        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<ICollection<FoodItemResponse>>> GetAllAsync()
    {
        var foodItems = await _context.FoodItems
            .Select(f => new FoodItemResponse(
                f.Id,
                f.Name,
                f.Description,
                f.Price,
                f.ImageUrl,
                f.QuantityAvailable,
                f.ExpiryTime,
                f.BusinessId
            ))
            .ToListAsync();

        return Result.Success<ICollection<FoodItemResponse>>(foodItems);
    }

    public async Task<Result<FoodItemResponse>> GetByIdAsync(Guid id)
    {
        var foodItem = await _context.FoodItems.FindAsync(id);

        if (foodItem is null)
            return Result.Failure<FoodItemResponse>(FoodItemErrors.NotFound);

        return Result.Success(new FoodItemResponse( id, foodItem.Name, foodItem.Description, foodItem.Price, foodItem.ImageUrl, foodItem.QuantityAvailable, foodItem.ExpiryTime, foodItem.BusinessId));
    }

    public async Task<Result> UpdateAsync(Guid id, FoodItemRequest request)
    {
        var currentUserId =  _httpContextAccessor.HttpContext?.User.GetUserId();

        if (string.IsNullOrWhiteSpace(currentUserId))
            return Result.Failure(BusinessErrors.Unauthorized);

        var foodItem = await _context.FoodItems.Include(f => f.Business).FirstOrDefaultAsync(f => f.Id == id);

        if (foodItem is null)
            return Result.Failure(FoodItemErrors.NotFound);

        // Check if the user is the owner of the business - only the business owner can update
        if (foodItem.Business.UserId != currentUserId)
            return Result.Failure(BusinessErrors.Forbidden);

        foodItem.Name = request.Name;
        foodItem.Description = request.Description;
        foodItem.Price = request.Price;
        foodItem.ImageUrl = request.ImageUrl;
        foodItem.QuantityAvailable = request.QuantityAvailable;
        foodItem.ExpiryTime = request.ExpiryTime;

        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
