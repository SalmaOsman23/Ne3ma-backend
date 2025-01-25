using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Neama.Core.Entities.Identity;
using Neama.Core.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Services
{
    public class TokenServices : ITokenService
    {
        public IConfiguration Configuration { get; }
        public TokenServices(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       

        public async Task<string> GetToken(AppUser user, UserManager<AppUser> userManager)
        {
            var authClaim = new List<Claim>()
       {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name,user.DisplayName),

       };


            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles) 
                authClaim.Add(new Claim(ClaimTypes.Role, role));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]));

            //Create Token
            var token = new JwtSecurityToken(

                //Register claims
                issuer: Configuration["JWT:ValidIssuer"],
                audience: Configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(Configuration["JWT:DurationInDays"])),
                //private claims
                claims: authClaim,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
