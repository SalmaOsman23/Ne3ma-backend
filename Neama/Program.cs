using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neama.Core.Entities.Identity;
using Neama.Repository.Identity;
using System;
using System.Threading.Tasks;

namespace Neama
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //create scopped services
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try

            {
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager, roleManager);
                await AppIdentityDbContextSeed.SeedRolesAsync(roleManager);

                var identitycontext = services.GetRequiredService<AppIdentityDbContext>();
                await identitycontext.Database.MigrateAsync();


            }
            catch (Exception ex)
            {
                var Logger = loggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "an error occured during apply Migration");

                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database");
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
