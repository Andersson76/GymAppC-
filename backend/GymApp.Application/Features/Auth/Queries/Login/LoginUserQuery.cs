using GymAppC.Application.Common.Models;
using GymAppC.Application.Dtos;
using MediatR;

namespace GymAppC.Application.Features.Auth.Queries.Login;

public sealed record LoginUserQuery(string Email, string Password)
    : IRequest<Result<AuthResponseDto>>;
