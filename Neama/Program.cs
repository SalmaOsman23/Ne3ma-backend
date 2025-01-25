using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neama.Core.Entities.Identity;
using Neama.Repository.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neama
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ///i use this way because StoreContext has only one constructor taht take options and i can not send options
            //To update Database i will make dependency injection for Dbcontext
            //host = Kesterel
            var host = CreateHostBuilder(args).Build();

            //create scopped services
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();//if an exception happen durin g Update
            try

            {
                


                var identitycontext = services.GetRequiredService<AppIdentityDbContext>();
                await identitycontext.Database.MigrateAsync();

                ////call seed in Program
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
            }
            catch (Exception ex)
            {
                var Logger = loggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "an error occured during apply Migration");
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
