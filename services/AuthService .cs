
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using webShop2.Infrastructure.Data;
using webShop2.dto.auth;
using webShop2.model;
using webShop2.model.enums;
using webShop2.services.core;

namespace webShop2.services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly ILogger<AuthService> _logger;
       



        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<UserRole> roleManager,
            IJwtService jwtService,
            IConfiguration configuration,
            AppDbContext context,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task<LoginResponseDto?> RegisterAsync(
            RegisterDto dto)
        {
            string? registrationKey =
                _configuration["AdminRegistration:Key"];

            if (string.IsNullOrWhiteSpace(registrationKey))
            {
                throw new InvalidOperationException(
                    "Admin registration key is not configured.");
            }

            byte[] expectedKey =
                Encoding.UTF8.GetBytes(registrationKey);

            byte[] providedKey =
                Encoding.UTF8.GetBytes(dto.RegistrationKey);

            if (expectedKey.Length != providedKey.Length ||
                !CryptographicOperations.FixedTimeEquals(
                    expectedKey,
                    providedKey))
            {
                _logger.LogWarning(
                    "Registration failed: invalid registration key.");

                return null;
            }

            string email = dto.Email.Trim();

            User? existingUser =
                await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                _logger.LogWarning(
                    "Registration failed: email already exists.");

                return null;
            }

            bool roleExists =
                await _roleManager.RoleExistsAsync(UserRoles.Admin);

            if (!roleExists)
            {
                _logger.LogError(
                    "Registration failed: Admin role does not exist.");

                throw new InvalidOperationException(
                    "Admin role does not exist. Run Identity role initialization.");
            }

            // Transaction protects the single-admin registration.
            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            IList<User> administrators =
                await _userManager.GetUsersInRoleAsync(
                    UserRoles.Admin);

            if (administrators.Count > 0)
            {
                _logger.LogWarning(
                    "Registration failed: administrator already exists.");

                return null;
            }

            User user = new User
            {
                UserName = email,
                Email = email,
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            IdentityResult createResult =
                await _userManager.CreateAsync(
                    user,
                    dto.Password);

            if (!createResult.Succeeded)
            {
                foreach (IdentityError error in createResult.Errors)
                {
                    _logger.LogWarning(
                        "Identity registration error: {Code} - {Description}",
                        error.Code,
                        error.Description);
                }

                return null;
            }

            IdentityResult roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    UserRoles.Admin);

            if (!roleResult.Succeeded)
            {
                foreach (IdentityError error in roleResult.Errors)
                {
                    _logger.LogWarning(
                        "Identity role error: {Code} - {Description}",
                        error.Code,
                        error.Description);
                }

                return null;
            }

            await transaction.CommitAsync();

            return GenerateLoginResponse(
                user,
                UserRoles.Admin);
        }

        public async Task<LoginResponseDto?> LoginAsync(
            LoginDto dto)
        {
            string email = dto.Email.Trim();

            User? user =
                await _userManager.FindByEmailAsync(email);

            if (user == null || !user.IsActive)
            {
                return null;
            }

            SignInResult signInResult =
                await _signInManager.CheckPasswordSignInAsync(
                    user,
                    dto.Password,
                    true);

            if (!signInResult.Succeeded)
            {
                return null;
            }

            bool isAdmin =
                await _userManager.IsInRoleAsync(
                    user,
                    UserRoles.Admin);

            if (!isAdmin)
            {
                return null;
            }

            return GenerateLoginResponse(
                user,
                UserRoles.Admin);
        }

        private LoginResponseDto GenerateLoginResponse(
            User user,
            string role)
        {
            int expirationMinutes =
                int.Parse(
                    _configuration["Jwt:ExpirationMinutes"]
                    ?? "60");

            DateTime expiresAt =
                DateTime.UtcNow.AddMinutes(
                    expirationMinutes);

            string token = _jwtService.GenerateToken(
                user.Id,
                user.Email ?? string.Empty,
                user.FirstName,
                user.LastName,
                role,
                expiresAt);

            return new LoginResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role
            };
        }


        public async Task<bool> DeleteUserAsync(string userId)
        {
            User? user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning(
                    "User deletion failed: user {UserId} not found.",
                    userId);

                return false;
            }

            IdentityResult result =
                await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    _logger.LogError(
                        "User deletion error: {Code} - {Description}",
                        error.Code,
                        error.Description);
                }

                return false;
            }

            _logger.LogInformation(
                "User {UserId} was successfully deleted.",
                userId);

            return true;
        }

    }
}
