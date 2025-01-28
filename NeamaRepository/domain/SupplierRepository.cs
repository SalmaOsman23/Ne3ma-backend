using Microsoft.EntityFrameworkCore;
using Neama.Core.Entities;
using Neama.Core.Repositories;
using Neama.Repository.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neama.Repository.domain
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppIdentityDbContext _context;

        public SupplierRepository(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Supplier>> GetActiveSuppliersAsync()
        {
            return await _context.Suppliers
                .Where(s => s.IsActive)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            var existing = await _context.Suppliers.FindAsync(supplier.Id);
            if (existing != null)
            {
                existing.Name = supplier.Name;
                existing.Address = supplier.Address;
                existing.Description = supplier.Description;
                existing.PickupStartTime = supplier.PickupStartTime;
                existing.PickupEndTime = supplier.PickupEndTime;
                existing.Latitude = supplier.Latitude;
                existing.Longitude = supplier.Longitude;

                _context.Suppliers.Update(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                supplier.IsActive = false; // Soft delete
                await _context.SaveChangesAsync();
            }
        }
    }
}
