using Microsoft.AspNetCore.Identity;
using Neama.Core.Entities.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neama.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles first
            await SeedRolesAsync(roleManager);

            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Salma Osman",
                    Email = "salmaosman55@gmail.com",
                    UserName = "salmaosman",
                    PhoneNumber = "0112233355"
                };

                var result = await userManager.CreateAsync(user, "Pa$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Consumer"); // Assign user to Consumer role
                }

                var error = result.Errors.First();
                Console.WriteLine(error.Description);
            }
        }

        // Seed Roles 
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            if (!await roleManager.RoleExistsAsync("Supplier"))
                await roleManager.CreateAsync(new IdentityRole("Supplier"));
        }

    }
}
