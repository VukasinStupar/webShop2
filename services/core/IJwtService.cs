
namespace webShop2.services.core
{
    public interface IJwtService
    {
        string GenerateToken(
            int userId,
            string email,
            string firstName,
            string lastName,
            string role,
            DateTime expiresAt);
    }
}
