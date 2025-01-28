using Microsoft.EntityFrameworkCore;
using Neama.Core.Entities;
using Neama.Core.Repositories;
using Neama.Repository.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neama.Repository.domain
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppIdentityDbContext _context;

        public OrderRepository(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.SurpriseBag)
                    .ThenInclude(sb => sb.Supplier)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
                .Include(o => o.SurpriseBag)
                    .ThenInclude(sb => sb.Supplier)
                .ToListAsync();
        }

        public async Task UpdateSurpriseBagQuantityAsync(int surpriseBagId, int quantityChange)
        {
            var surpriseBag = await _context.SurpriseBags.FindAsync(surpriseBagId);
            if (surpriseBag != null)
            {
                surpriseBag.QuantityAvailable += quantityChange;
                _context.SurpriseBags.Update(surpriseBag);
                await _context.SaveChangesAsync();
            }
        }
    }
}
