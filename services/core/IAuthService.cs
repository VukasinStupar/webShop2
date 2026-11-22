
using webShop2.dto.auth;

namespace webShop2.services.core
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> RegisterAsync(RegisterDto dto);

        Task<LoginResponseDto?> LoginAsync(LoginDto dto);

        
        Task<bool> DeleteUserAsync(string userId);
    }
}
