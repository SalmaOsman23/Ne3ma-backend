using Ne3ma.Entities;

namespace Ne3ma.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Admin admin);

    }
}
