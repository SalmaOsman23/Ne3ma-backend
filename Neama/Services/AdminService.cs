using Ne3ma.Entities;
using Org.BouncyCastle.Crypto.Generators;

namespace Ne3ma.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AdminService(ApplicationDbContext context, IJwtTokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string?> Authenticate(string email, string password)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                return null;

            return _tokenGenerator.GenerateToken(admin);
        }

        public async Task<Admin> CreateAdmin(Admin admin)
        {
            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(admin.PasswordHash);
            admin.CreatedAt = DateTime.UtcNow;
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<List<Admin>> GetAllAdmins()
        {
            return await _context.Admins.ToListAsync();
        }

        public async Task<Admin?> GetAdminById(int id)
        {
            return await _context.Admins.FindAsync(id);
        }

        public async Task<bool> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return false;

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
