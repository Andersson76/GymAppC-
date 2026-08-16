using GymAppC.Application.Interfaces;

namespace GymAppC.Application.Tests.TestDoubles;

internal sealed class FakePasswordHasher : IPasswordHasher
{
    public byte[] Hash { get; init; } = [1, 2, 3];
    public byte[] Salt { get; init; } = [4, 5, 6];
    public int HashPasswordCalls { get; private set; }
    public string? LastPassword { get; private set; }

    public (byte[] Hash, byte[] Salt) HashPassword(string password)
    {
        HashPasswordCalls++;
        LastPassword = password;
        return (Hash, Salt);
    }

    public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt) => false;
}
