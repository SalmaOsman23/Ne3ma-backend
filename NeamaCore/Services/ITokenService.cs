using Microsoft.AspNetCore.Identity;
using Neama.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services
{
    public interface ITokenService
    {
        Task<string> GetToken(AppUser user,UserManager<AppUser> userManager);
    }
}
