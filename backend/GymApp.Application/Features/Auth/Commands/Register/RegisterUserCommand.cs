using GymAppC.Application.Common.Models;
using MediatR;

namespace GymAppC.Application.Features.Auth.Commands.Register;

public sealed record RegisterUserCommand(string Name, string Email, string Password)
    : IRequest<Result<string>>;
