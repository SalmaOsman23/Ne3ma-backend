using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Neama.Core.Entities.Identity;
using Neama.Repository.Identity;
using System.Text;

namespace Neama.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://localhost:5001",
                ValidAudience = "YourApp",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourStrongSecretKey"))
            };
        });
            return services;
        }
    }
}
