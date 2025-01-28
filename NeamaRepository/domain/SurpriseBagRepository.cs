using Microsoft.EntityFrameworkCore;
using Neama.Core.Entities;
using Neama.Core.Repositories;
using Neama.Repository.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neama.Repository.domain
{
    public class SurpriseBagRepository : ISurpriseBagRepository
    {
        private readonly AppIdentityDbContext _context;

        public SurpriseBagRepository(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<SurpriseBag>> GetAvailableSurpriseBagsAsync()
        {
            return await _context.SurpriseBags
                .Where(sb => sb.QuantityAvailable > 0)
                .Include(sb => sb.Supplier)
                .ToListAsync();
        }

        public async Task<SurpriseBag> GetSurpriseBagByIdAsync(int id)
        {
            return await _context.SurpriseBags
                .Include(sb => sb.Supplier)
                .FirstOrDefaultAsync(sb => sb.Id == id);
        }

        public async Task AddSurpriseBagAsync(SurpriseBag surpriseBag)
        {
            await _context.SurpriseBags.AddAsync(surpriseBag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSurpriseBagAsync(SurpriseBag surpriseBag)
        {
            var existing = await _context.SurpriseBags.FindAsync(surpriseBag.Id);
            if (existing != null)
            {
                existing.Title = surpriseBag.Title;
                existing.Description = surpriseBag.Description;
                existing.Price = surpriseBag.Price;
                existing.PickupTime = surpriseBag.PickupTime;
                existing.QuantityAvailable = surpriseBag.QuantityAvailable;

                _context.SurpriseBags.Update(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSurpriseBagAsync(int id)
        {
            var bag = await _context.SurpriseBags.FindAsync(id);
            if (bag != null)
            {
                bag.QuantityAvailable = 0; // Soft delete
                await _context.SaveChangesAsync();
            }
        }
    }
}
