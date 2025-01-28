using Microsoft.AspNetCore.Identity;

namespace Neama.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

    }
}
