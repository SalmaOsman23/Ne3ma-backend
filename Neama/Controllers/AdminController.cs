using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ne3ma.Contracts.Admins;
using Ne3ma.Entities;
using Ne3ma.Services;

namespace Ne3ma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // 🔐 SuperAdmin Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginDto loginDto)
        {
            var token = await _adminService.Authenticate(loginDto.Email, loginDto.Password);
            if (token == null) return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }

        // 🔐 SuperAdmin Creates Another Admin
        [HttpPost("create")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateAdmin([FromBody] Admin newAdmin)
        {
            var createdAdmin = await _adminService.CreateAdmin(newAdmin);
            return CreatedAtAction(nameof(GetAdminById), new { id = createdAdmin.Id }, createdAdmin);
        }

        // 🔐 Get All Admins (SuperAdmin Only)
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAdmins();
            return Ok(admins);
        }

        // 🔐 Get Admin By ID
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _adminService.GetAdminById(id);
            if (admin == null) return NotFound();
            return Ok(admin);
        }

        // 🔐 Delete Admin (Only SuperAdmin Can Delete)
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var success = await _adminService.DeleteAdmin(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}