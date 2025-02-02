using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ne3ma.Contracts.Users;
using Ne3ma.Services;
using Neama.Extentions;

namespace Ne3ma.Controllers;
[Route("[controller]")]
[ApiController]
[Authorize]

public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService userService = userService;

    [HttpGet("")]
    public async Task<IActionResult> Info()
    {
        var result = await userService.GetProfileAsync(User.GetUserId()!);

        return Ok(result.Value);
    }

    [HttpPut("info")]
    public async Task<IActionResult> Info([FromBody] UpdateProfileRequest request)
    {
        await userService.UpdateProfileAsync(User.GetUserId()!, request);

        return NoContent();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await userService.ChangePasswordAsync(User.GetUserId()!, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
