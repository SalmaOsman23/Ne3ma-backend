using Ne3ma.Entities;

namespace Ne3ma.Services
{
    public interface IAdminService
    {
        Task<string?> Authenticate(string email, string password);
        Task<Admin> CreateAdmin(Admin admin);
        Task<List<Admin>> GetAllAdmins();
        Task<Admin?> GetAdminById(int id);
        Task<bool> DeleteAdmin(int id);
    }
}
