using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neama.Core.Entities;
using Neama.Core.Entities.Identity;
using Neama.Dtos;
using Neama.Errors;
using Neama.Repository.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neama.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurpriseBagsController : ControllerBase
    {
        private readonly AppIdentityDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SurpriseBagsController(AppIdentityDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult<SurpriseBagResponseDto>> CreateSurpriseBag(SurpriseBagCreateDto model)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (supplier == null)
            {
                return BadRequest(new ApiResponse(400, "You need to register as a supplier first."));
            }

            var surpriseBag = new SurpriseBag
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                PickupTime = model.PickupTime,
                QuantityAvailable = model.QuantityAvailable,
                SupplierId = supplier.Id
            };

            _context.SurpriseBags.Add(surpriseBag);
            await _context.SaveChangesAsync();

            return Ok(new SurpriseBagResponseDto
            {
                Id = surpriseBag.Id,
                Title = surpriseBag.Title,
                Description = surpriseBag.Description,
                Price = surpriseBag.Price,
                PickupTime = surpriseBag.PickupTime,
                QuantityAvailable = surpriseBag.QuantityAvailable,
                SupplierId = surpriseBag.SupplierId
            });
        }

        [HttpGet("nearby")]
        public async Task<ActionResult<ICollection<SurpriseBagResponseDto>>> GetNearbySurpriseBags([FromQuery] double latitude, [FromQuery] double longitude)
        {
            var maxDistance = 5.0; // 5km
            var query = _context.SurpriseBags
                .Where(sb => sb.QuantityAvailable > 0)
                .Where(sb =>
                    6371 * Math.Acos(
                        Math.Clamp(
                            Math.Cos(Math.PI / 180 * latitude) *
                            Math.Cos(Math.PI / 180 * sb.Supplier.Latitude) *
                            Math.Cos(Math.PI / 180 * (sb.Supplier.Longitude - longitude)) +
                            Math.Sin(Math.PI / 180 * latitude) *
                            Math.Sin(Math.PI / 180 * sb.Supplier.Latitude),
                            -1, 1
                        )
                    ) <= maxDistance
                )
                .Select(sb => new SurpriseBagResponseDto
                {
                    Id = sb.Id,
                    Title = sb.Title,
                    Description = sb.Description,
                    Price = sb.Price,
                    PickupTime = sb.PickupTime,
                    QuantityAvailable = sb.QuantityAvailable,
                    SupplierId = sb.SupplierId
                });

            return Ok(await query.ToListAsync());
        }
    }
}
