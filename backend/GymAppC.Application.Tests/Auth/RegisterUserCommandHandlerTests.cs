using GymAppC.Application.Features.Auth.Commands.Register;
using GymAppC.Application.Tests.TestDoubles;
using GymAppC.Domain.Constants;

namespace GymAppC.Application.Tests.Auth;

public sealed class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_NormalizesInput_AssignsUserRole_AndPersistsUser()
    {
        var repository = new FakeUserRepository();
        var passwordHasher = new FakePasswordHasher
        {
            Hash = [10, 20],
            Salt = [30, 40]
        };
        var handler = new RegisterUserCommandHandler(repository, passwordHasher);

        var result = await handler.Handle(
            new RegisterUserCommand("  Ada Lovelace  ", "  ADA@Example.COM  ", "secret1"),
            CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.Equal("Användare registrerad.", result.Value);
        Assert.Equal("ada@example.com", repository.CheckedEmail);

        var user = Assert.IsType<GymAppC.Domain.Entities.User>(repository.AddedUser);
        Assert.Equal("Ada Lovelace", user.Name);
        Assert.Equal("ada@example.com", user.Email);
        Assert.Equal(AppRoles.User, user.Role);
        Assert.Equal(new byte[] { 10, 20 }, user.PasswordHash);
        Assert.Equal(new byte[] { 30, 40 }, user.PasswordSalt);
        Assert.Equal("secret1", passwordHasher.LastPassword);
        Assert.Equal(1, passwordHasher.HashPasswordCalls);
        Assert.Equal(1, repository.TryAddCalls);
        Assert.Equal(0, repository.SaveChangesCalls);
    }

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ReturnsFailureWithoutPersisting()
    {
        var repository = new FakeUserRepository { EmailExists = true };
        var passwordHasher = new FakePasswordHasher();
        var handler = new RegisterUserCommandHandler(repository, passwordHasher);

        var result = await handler.Handle(
            new RegisterUserCommand("Existing User", " Existing@Example.COM ", "secret1"),
            CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Null(result.Value);
        Assert.Equal("Användaren finns redan.", result.Error);
        Assert.Equal("existing@example.com", repository.CheckedEmail);
        Assert.Null(repository.AddedUser);
        Assert.Equal(0, passwordHasher.HashPasswordCalls);
        Assert.Equal(0, repository.TryAddCalls);
        Assert.Equal(0, repository.SaveChangesCalls);
    }

    [Fact]
    public async Task Handle_WhenConcurrentInsertWins_ReturnsControlledFailure()
    {
        var repository = new FakeUserRepository { TryAddSucceeds = false };
        var passwordHasher = new FakePasswordHasher();
        var handler = new RegisterUserCommandHandler(repository, passwordHasher);

        var result = await handler.Handle(
            new RegisterUserCommand("Concurrent User", "same@example.com", "secret1"),
            CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal("Användaren finns redan.", result.Error);
        Assert.Equal(1, passwordHasher.HashPasswordCalls);
        Assert.Equal(1, repository.TryAddCalls);
    }
}
