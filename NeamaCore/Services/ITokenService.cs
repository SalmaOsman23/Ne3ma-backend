using Microsoft.AspNetCore.Identity;
using Neama.Core.Entities.Identity;
using System.Threading.Tasks;

namespace Neama.Core.Services
{
    public interface ITokenService
    {
        Task<string> GetToken(AppUser user, UserManager<AppUser> userManager);
    }
}
