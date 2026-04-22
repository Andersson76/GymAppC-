using GymAppC.Application.Dtos;
using GymAppC.Application.Interfaces;
using GymAppC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GymAppC.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<(bool Success, string Message)> RegisterAsync(RegisterUserDto dto)
        {
            var normalizedEmail = dto.Email.Trim().ToLower();
            var emailExists = await _userRepository.EmailExistsAsync(normalizedEmail);

            if (emailExists)
            {
                return (false, "Användaren finns redan.");
            }

            CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Name = dto.Name.Trim(),
                Email = normalizedEmail,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return (true, "Användare registrerad.");

        }
        public async Task<(bool Success, string Message, AuthResponseDto? Response)> LoginAsync(LoginUserDto dto)
        {
            var normalizedEmail = dto.Email.Trim().ToLower();
            var user = await _userRepository.GetByEmailAsync(normalizedEmail);

            if (user is null)
            {
                return (false, "Fel e-post eller lösenord.", null);
            }

            if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return (false, "Fel e-post eller lösenord.", null);
            }

            var response = new AuthResponseDto
            {
                Email = user.Email,
                Name = user.Name,
                Token = _tokenService.CreateToken(user)
            };

            return (true, "Inloggning lyckades.", response);

        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(storedHash);
        }
    }
}
