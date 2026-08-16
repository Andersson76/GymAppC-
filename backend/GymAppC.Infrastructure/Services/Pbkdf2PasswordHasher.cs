using System.Security.Cryptography;
using System.Text;
using GymAppC.Application.Interfaces;

namespace GymAppC.Infrastructure.Services;

public sealed class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int Iterations = 210_000;
    private const int SaltSize = 16;
    private const int HashSize = 64;

    public (byte[] Hash, byte[] Salt) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA512,
            HashSize);

        return (hash, salt);
    }

    public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        var pbkdf2Hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            storedSalt,
            Iterations,
            HashAlgorithmName.SHA512,
            storedHash.Length);

        if (CryptographicOperations.FixedTimeEquals(pbkdf2Hash, storedHash))
        {
            return true;
        }

        // Existing installations used HMACSHA512. Keep those users able to log in.
        using var legacyHasher = new HMACSHA512(storedSalt);
        var legacyHash = legacyHasher.ComputeHash(Encoding.UTF8.GetBytes(password));
        return CryptographicOperations.FixedTimeEquals(legacyHash, storedHash);
    }
}
