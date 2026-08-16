using GymAppC.Application.Dtos;
using MediatR;

namespace GymAppC.Application.Features.Users.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery(int UserId) : IRequest<CurrentUserDto?>;
