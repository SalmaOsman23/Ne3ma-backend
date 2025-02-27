using Ne3ma.Contracts.Businesses;

namespace Ne3ma.Services;

public interface IBusinessService
{
    Task<Result<BusinessResponse>> CreateBusinessAsync(string userId, BusinessRequest request);
    Task<Result> UpdateBusinessAsync(Guid businessId, string userId, BusinessRequest request);
    Task<Result> DeleteBusinessAsync(Guid businessId, string userId);
    Task<Result<ICollection<BusinessResponse>>> GetAllBusinessesAsync();
    Task<Result<BusinessResponse>> GetBusinessByIdAsync(Guid businessId);
    Task<Result> ApproveBusinessAsync(Guid businessId);
}
