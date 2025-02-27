using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ne3ma.Contracts.Businesses;
using Ne3ma.Services;
using Neama.Extentions;

namespace Ne3ma.Controllers;
[Route("[controller]")]
[ApiController]
public class BusinessController(IBusinessService businessService) : ControllerBase
{
    private readonly IBusinessService _businessService = businessService;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBusiness([FromBody] BusinessRequest request)
    {
        var result = await _businessService.CreateBusinessAsync(User.GetUserId()!, request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{businessId}")]
    [Authorize]
    public async Task<IActionResult> UpdateBusiness(Guid businessId, [FromBody] BusinessRequest request)
    {
        var result = await _businessService.UpdateBusinessAsync(businessId, User.GetUserId()!, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{businessId}")]
    [Authorize]
    public async Task<IActionResult> DeleteBusiness(Guid businessId)
    {
        var result = await _businessService.DeleteBusinessAsync(businessId, User.GetUserId()!);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBusinesses()
    {
        var result = await _businessService.GetAllBusinessesAsync();
        return Ok(result.Value);
    }

    [HttpGet("{businessId}")]
    public async Task<IActionResult> GetBusinessById(Guid businessId)
    {
        var result = await _businessService.GetBusinessByIdAsync(businessId);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{businessId}/approve")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveBusiness(Guid businessId)
    {
        var result = await _businessService.ApproveBusinessAsync(businessId);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
