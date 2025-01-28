using Neama.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neama.Core.Repositories
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task<List<Supplier>> GetActiveSuppliersAsync();
        Task AddSupplierAsync(Supplier supplier);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(int id);
    }
}
