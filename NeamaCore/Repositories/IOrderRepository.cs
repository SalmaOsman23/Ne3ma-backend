using Neama.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neama.Core.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int id);
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task UpdateSurpriseBagQuantityAsync(int surpriseBagId, int quantityChange);
    }
}
