using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neama.Core.Entities;
using Neama.Core.Entities.Identity;
using Neama.Dtos;
using Neama.Errors;
using Neama.Repository.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Neama.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly AppIdentityDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SuppliersController(AppIdentityDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("register")]
        //[Authorize(Roles = "Supplier")]
        public async Task<ActionResult<SupplierResponseDto>> RegisterSupplier(SupplierCreateDto supplierDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.UserId == user.Id);

            if (existingSupplier is not null)
            {
                return BadRequest(new ApiResponse(400, "You already have a registered supplier."));
            }

            var supplier = new Supplier
            {
                Name = supplierDto.Name,
                Address = supplierDto.Address,
                Description = supplierDto.Description,
                PickupStartTime = supplierDto.PickupStartTime,
                PickupEndTime = supplierDto.PickupEndTime,
                Latitude = supplierDto.Latitude,
                Longitude = supplierDto.Longitude,
                UserId = user.Id
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return Ok(new SupplierResponseDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Address = supplier.Address,
                Description = supplier.Description,
                PickupStartTime = supplier.PickupStartTime,
                PickupEndTime = supplier.PickupEndTime,
                Latitude = supplier.Latitude,
                Longitude = supplier.Longitude,
                UserId = supplier.UserId
            });
        }

        [HttpGet("my-suppliers")]
        //[Authorize(Roles = "Supplier")]
        public async Task<ActionResult<ICollection<SupplierResponseDto>>> GetMySuppliers()
        {
            //  var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var suppliers = await _context.Suppliers
                .Where(s => s.UserId == user.Id)
                .Select(s => new SupplierResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    Description = s.Description,
                    PickupStartTime = s.PickupStartTime,
                    PickupEndTime = s.PickupEndTime,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    UserId = s.UserId
                }).ToListAsync();

            return Ok(suppliers);
        }
    }
}
