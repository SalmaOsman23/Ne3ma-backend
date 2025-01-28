using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neama.Core.Entities.Identity;
using Neama.Core.Services;
using Neama.Dtos;
using Neama.Errors;
using Neama.Helper;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Neama.Controllers
{

    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;



        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;

        }

        [HttpPost("login")]//Post: /api//Account/login
        public async Task<ActionResult<UserDto>> login(LoginDto loginDto)
        {
            AppUser user = null;
            if (Regex.IsMatch(loginDto.EmailOrPhone, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                // Login using email
                user = await _userManager.FindByEmailAsync(loginDto.EmailOrPhone);
            }
            else
            {
                // Login using phone number
                user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == loginDto.EmailOrPhone);
            }

            if (user == null) return Unauthorized(new ApiResponse(401));

            // Validate password
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            // Return user details and token
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.GetToken(user, _userManager)
            });
        }



        [HttpPost("register")]//Post: /api//Account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.Email.Split("@")[0]
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Assign the role
            await _userManager.AddToRoleAsync(user, registerDto.UserRole);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.GetToken(user, _userManager)
            });
        }



        //[HttpPost("forgetpassword")]
        //public async Task<IActionResult> ForgetPassword(ForgetPasswordDto request)
        //{
        //    var user = await _userManager.FindByEmailAsync(User.Identity.Name);

        //    if (user == null)
        //    {
        //       return Unauthorized(new ApiResponse(401));
        //    }

        //    if (request.NewPassword != request.ConfirmNewPassword)
        //    {
        //       return BadRequest(new ApiResponse(400));
        //    }

        //    // Update the user's password directly
        //    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);
        //    await _userManager.UpdateAsync(user);

        //    return Ok(new UserDto()
        //    {
        //        DisplayName = user.DisplayName,
        //        Email = user.Email,
        //        Token = await _tokenService.GetToken(user, _userManager)
        //    });



        //}

        //[HttpPost("resetpassword")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();
        //    var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        //    if (user is null) return BadRequest("Invalid request");

        //    var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token!, resetPasswordDto.NewPassword!);
        //    if (!result.Succeeded)
        //    {
        //        var errors = result.Errors.Select(e => e.Description);
        //        return BadRequest(new { Errors = errors });
        //    }
        //    return Ok();

        //}



        //[HttpPost("resetpassword")]
        //public async Task<IActionResult> ResetPasswordWithoutToken([FromBody] ResetPasswordDto resetPasswordDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    // Ensure passwords match
        //    if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
        //        return BadRequest("Passwords do not match.");

        //    // Find the user by email
        //    var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        //    if (user is null)
        //        return BadRequest("Invalid request");



        //    // Continue with your reset password logic

        //    // Remove the user's existing password hash
        //    var removePasswordResult = await _userManager.RemovePasswordAsync(user);
        //    if (!removePasswordResult.Succeeded)
        //    {
        //        var errors = removePasswordResult.Errors.Select(e => e.Description);
        //        return BadRequest(new { Errors = errors });
        //    }

        //    // Set the new password
        //    var result = await _userManager.AddPasswordAsync(user, resetPasswordDto.NewPassword!);
        //    if (!result.Succeeded)
        //    {
        //        var errors = result.Errors.Select(e => e.Description);
        //        return BadRequest(new { Errors = errors });
        //    }

        //    return Ok("Password reset successfully without a token.");
        //}



        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPasswordWithoutToken([FromBody] ResetPasswordDto resetPasswordDto)
        {
            // Check model state
            if (!ModelState.IsValid)
                return BadRequest("Invalid input.");

            // Ensure passwords match
            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
                return BadRequest("Passwords do not match.");

            // Check if input is null or empty
            if (string.IsNullOrWhiteSpace(resetPasswordDto.EmailOrPhone))
                return BadRequest("Email or phone number is required.");

            AppUser user = null;

            // Determine if input is email or phone number
            if (CheckEmailorPhoneForResetPassword.IsValidEmail(resetPasswordDto.EmailOrPhone))
            {
                user = await _userManager.FindByEmailAsync(resetPasswordDto.EmailOrPhone);
            }
            else if (CheckEmailorPhoneForResetPassword.IsValidPhoneNumber(resetPasswordDto.EmailOrPhone))
            {
                user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == resetPasswordDto.EmailOrPhone);
            }
            else
            {
                return BadRequest("Invalid email or phone number format.");
            }

            // Check if user exists
            if (user == null)
                return BadRequest("User not found.");

            // Remove the user's existing password
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                var errors = removePasswordResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            // Set the new password
            var addPasswordResult = await _userManager.AddPasswordAsync(user, resetPasswordDto.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                var errors = addPasswordResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            return Ok("Password reset successfully.");
        }
    }
}
