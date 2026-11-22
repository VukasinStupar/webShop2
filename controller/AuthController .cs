
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webShop2.dto.auth;
using webShop2.services.core;
using System.Text.Json;
using System.Security.Claims;
using webShop2.model.enums;

namespace webShop2.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<LoginResponseDto>> Register(
            RegisterDto dto)
        {
            Console.WriteLine("Register DTO:");
            Console.WriteLine(dto);

            
            LoginResponseDto? response =
                await _authService.RegisterAsync(dto);

            Console.WriteLine("Login Response:");
            Console.WriteLine(response);

            if (response == null)
            {
                return BadRequest(new
                {
                    message = "Registration failed."
                });
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(
            LoginDto dto)
        {
            LoginResponseDto? response =
                await _authService.LoginAsync(dto);

            if (response == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            return Ok(response);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            Console.WriteLine("DELETE ACCOUNT ENDPOINT REACHED");



            string? userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            Console.WriteLine($"User ID: {userId}");
            Console.WriteLine($"User Role: {User.FindFirstValue(ClaimTypes.Role)}");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            bool deleted = await _authService.DeleteUserAsync(userId);



            Console.WriteLine($"Deletion result: {deleted}");

            if (!deleted)
            {
                return NotFound("User not found or deletion failed.");
            }



            return Ok("Admin account successfully deleted.");
        }
    }
}
