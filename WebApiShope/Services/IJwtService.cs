using DTO;

namespace Services
{
    public interface IJwtService
    {
        string GenerateToken(UserDTO user, string username, string password);
    }
}
