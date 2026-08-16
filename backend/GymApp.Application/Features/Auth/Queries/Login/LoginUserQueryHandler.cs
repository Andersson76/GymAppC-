using AutoMapper;
using GymAppC.Application.Common.Models;
using GymAppC.Application.Dtos;
using GymAppC.Application.Interfaces;
using MediatR;

namespace GymAppC.Application.Features.Auth.Queries.Login;

public sealed class LoginUserQueryHandler
    : IRequestHandler<LoginUserQuery, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public LoginUserQueryHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        LoginUserQuery request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null ||
            !_passwordHasher.VerifyPassword(
                request.Password,
                user.PasswordHash,
                user.PasswordSalt))
        {
            return Result<AuthResponseDto>.Failure("Fel e-post eller lösenord.");
        }

        var response = _mapper.Map<AuthResponseDto>(user);
        response.Token = _tokenService.CreateToken(user);

        return Result<AuthResponseDto>.Success(response);
    }
}
