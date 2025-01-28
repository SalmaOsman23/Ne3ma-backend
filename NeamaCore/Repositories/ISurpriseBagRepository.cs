using Neama.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neama.Core.Repositories
{
    public interface ISurpriseBagRepository
    {
        Task<ICollection<SurpriseBag>> GetAvailableSurpriseBagsAsync();
        Task<SurpriseBag> GetSurpriseBagByIdAsync(int id);
        Task AddSurpriseBagAsync(SurpriseBag surpriseBag);
        Task UpdateSurpriseBagAsync(SurpriseBag surpriseBag);
        Task DeleteSurpriseBagAsync(int id);
    }
}
