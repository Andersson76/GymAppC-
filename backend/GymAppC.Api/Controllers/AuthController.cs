using GymAppC.Api.Data;
using GymAppC.Api.Dtos;
using GymAppC.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace GymAppC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var normalizedEmail = dto.Email.Trim().ToLowerInvariant();

            if (_context.Users.Any(u => u.Email == normalizedEmail))
            {
                return BadRequest(new { message = "Email already exists." });
            }

            var user = new User
            {
                Email = normalizedEmail,
                PasswordHash = HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new
            {
                message = "User registered successfully.",
                user = new
                {
                    user.Id,
                    user.Email
                }
            });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
