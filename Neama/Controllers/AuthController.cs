using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ne3ma.Contracts.Authentication;
using Ne3ma.Services;
using Neama.Authentication;

namespace Ne3ma.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, ILogger<AuthController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Logging with email: {email} and password: {password}", request.Email, request.Password);

        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        //if (!authResult.IsSuccess)
        //    return authResult.ToProblem();

        //var token = authResult.Value!.Token;
        //var refreshToken = authResult.Value.RefreshToken;

        //// Store token in Cookies
        //Response.Cookies.Append("AuthToken", token, new CookieOptions
        //{
        //    HttpOnly = true, // Security: Prevent JavaScript access
        //    Secure = true, // Only allow over HTTPS
        //    SameSite = SameSiteMode.Strict,
        //    Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes)
        //});

        //Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
        //{
        //    HttpOnly = true,
        //    Secure = true,
        //    SameSite = SameSiteMode.Strict,
        //    Expires = DateTime.UtcNow.AddDays(14) // Refresh token expiration time
        //});


        return authResult.IsSuccess
            ? Ok(authResult.Value) : authResult.ToProblem();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess
            ? Ok(authResult.Value) : authResult.ToProblem();
    }

    [HttpPut("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess
            ? Ok() : result.ToProblem();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ComfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var result = await _authService.ConfirmEmailAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("resend-confirm-email")]
    public async Task<IActionResult> ResendConfirmaitonEmail([FromBody] ResendConfirmationEmailRequest request)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetPasswordCodeAsync(request.Email);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
