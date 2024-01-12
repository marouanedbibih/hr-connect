using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.Core.Context;
using backend.Core.Entities;
using backend.Core.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Find the user in the database
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);

                // Check if the user exists
                if (user == null)
                {
                    return Unauthorized(new { message = "User not found" });
                }

                // Check if the password is correct
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                {
                    return Unauthorized(new { message = "Invalid password" });
                }

                // Check if the user is active
                if (!user.IsActive)
                {
                    return Unauthorized(new { message = "User is not active" });
                }

                // Generate token
                var token = GenerateRandomString(64);

                // Return user information and token
                return Ok(new
                {
                    user,
                    Token = token,
                    message = "Your login successfuly"
                });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error", errorDetails = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                stringBuilder.Append(chars[index]);
            }

            return stringBuilder.ToString();
        }
    }
}
