using GymAppC.Application.Common.Models;
using GymAppC.Application.Interfaces;
using GymAppC.Domain.Constants;
using GymAppC.Domain.Entities;
using MediatR;

namespace GymAppC.Application.Features.Auth.Commands.Register;

public sealed class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<string>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        if (await _userRepository.EmailExistsAsync(normalizedEmail, cancellationToken))
        {
            return Result<string>.Failure("Användaren finns redan.");
        }

        var (passwordHash, passwordSalt) = _passwordHasher.HashPassword(request.Password);
        var user = new User
        {
            Name = request.Name.Trim(),
            Email = normalizedEmail,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = AppRoles.User
        };

        if (!await _userRepository.TryAddAsync(user, cancellationToken))
        {
            return Result<string>.Failure("Användaren finns redan.");
        }

        return Result<string>.Success("Användare registrerad.");
    }
}
