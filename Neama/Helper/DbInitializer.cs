using Ne3ma.Entities;
using System;

namespace Ne3ma.Helper
{
    public class DbInitializer
    {
        public static void SeedSuperAdmin(ApplicationDbContext context)
        {
            if (!context.Admins.Any(a => a.Role == "SuperAdmin"))
            {
                var superAdmin = new Admin
                {
                    FullName = "Salma Osman",
                    Email = "salmaosmann21@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Salmaosman123#"),
                    Role = "SuperAdmin"
                };

                context.Admins.Add(superAdmin);
                context.SaveChanges();
            }
        }
    }
}
