using Microsoft.AspNetCore.Identity;
using Neama.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())//true => if there is No Users in DB  (Any return false if there is no User)
            {
                var user = new AppUser()
                {
                    DisplayName = "Salma Osman",
                    Email = "salmaosman55@gmail.com",
                    UserName = "salmaosman",
                    PhoneNumber = "0112233355"
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }

    }
}
