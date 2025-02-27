using Microsoft.EntityFrameworkCore;
using Ne3ma.Contracts.Businesses;
using Ne3ma.Entities;
using Ne3ma.Errors;

namespace Ne3ma.Services;

public class BusinessService(ApplicationDbContext context) : IBusinessService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<BusinessResponse>> CreateBusinessAsync(string userId, BusinessRequest request)
    {
        var business = new Business
        {
            Name = request.Name,
            Address = request.Address,
            Location = new NetTopologySuite.Geometries.Point(request.Longitude, request.Latitude) { SRID = 4326 },
            Phone = request.Phone,
            Description = request.Description,
            UserId = userId,
            IsApproved = false // Default not approved

        };

        await _context.AddAsync(business);
        await _context.SaveChangesAsync();

        return Result.Success(new BusinessResponse(
            business.Id, business.Name, business.Address, business.Phone, business.Description, business.IsApproved));
    }

    public async Task<Result> UpdateBusinessAsync(Guid businessId, string userId, BusinessRequest request)
    {
        var business = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == businessId);

        if (business is null)
            return Result.Failure(BusinessErrors.NotFound);

        if (business.UserId != userId)
            return Result.Failure(BusinessErrors.Unauthorized);

        business.Name = request.Name;
        business.Address = request.Address;
        business.Location = new NetTopologySuite.Geometries.Point(request.Longitude, request.Latitude) { SRID = 4326 };
        business.Phone = request.Phone;
        business.Description = request.Description;

        await _context.SaveChangesAsync();

        return Result.Success();

    }

    public async Task<Result> DeleteBusinessAsync(Guid businessId, string userId)
    {
        var business = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == businessId && x.UserId == userId);

        if (business is null)
            return Result.Failure(BusinessErrors.NotFound);

        _context.Remove(business);

        await _context.SaveChangesAsync();

        return Result.Success();

    }

    public async Task<Result<ICollection<BusinessResponse>>> GetAllBusinessesAsync()
    {
        var businesses = await _context.Businesses
            //.Where(x => x.IsApproved)
            .Select(x => new BusinessResponse(x.Id, x.Name, x.Address, x.Phone, x.Description, x.IsApproved))
            .ToListAsync();

        return Result.Success<ICollection<BusinessResponse>>(businesses);
    }

    public async Task<Result<BusinessResponse>> GetBusinessByIdAsync(Guid businessId)
    {
        var business = await _context.Businesses.FindAsync(businessId);

        if (business is null)
            return Result.Failure<BusinessResponse>(BusinessErrors.NotFound);

        return Result.Success(new BusinessResponse(
            businessId, business.Name, business.Address, business.Phone, business.Description, business.IsApproved));
    }

    public async Task<Result> ApproveBusinessAsync(Guid businessId)
    {
        var business = await _context.Businesses.FindAsync(businessId);

        if (business is null)
            return Result.Failure(BusinessErrors.NotFound);

        business.IsApproved = !business.IsApproved; 

        await _context.SaveChangesAsync();

        return Result.Success();

    }
}
