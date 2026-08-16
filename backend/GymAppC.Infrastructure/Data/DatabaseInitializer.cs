using GymAppC.Application.Interfaces;
using GymAppC.Domain.Constants;
using GymAppC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace GymAppC.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task SeedAdminAsync(
        AppDbContext context,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var email = configuration["AdminSeed:Email"]?.Trim().ToLowerInvariant();
        var password = configuration["AdminSeed:Password"];
        var name = configuration["AdminSeed:Name"]?.Trim();

        var hasAnySeedValue = !string.IsNullOrWhiteSpace(email) ||
            !string.IsNullOrWhiteSpace(password) ||
            !string.IsNullOrWhiteSpace(name);

        if (!hasAnySeedValue)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            !MailAddress.TryCreate(email, out _) ||
            email.Length > 256 ||
            password.Length is < 12 or > 128 ||
            name?.Length > 100)
        {
            throw new InvalidOperationException(
                "Admin seed requires a valid email, a 12-128 character password and a name of at most 100 characters.");
        }

        var (passwordHash, passwordSalt) = passwordHasher.HashPassword(password);
        var adminName = string.IsNullOrWhiteSpace(name) ? "GymApp Admin" : name;
        var existingUser = await context.Users.SingleOrDefaultAsync(
            user => user.Email == email,
            cancellationToken);

        if (existingUser is not null)
        {
            if (existingUser.Role == AppRoles.Admin)
            {
                return;
            }

            existingUser.Name = adminName;
            existingUser.PasswordHash = passwordHash;
            existingUser.PasswordSalt = passwordSalt;
            existingUser.Role = AppRoles.Admin;
        }
        else
        {
            context.Users.Add(new User
            {
                Email = email,
                Name = adminName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = AppRoles.Admin
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
