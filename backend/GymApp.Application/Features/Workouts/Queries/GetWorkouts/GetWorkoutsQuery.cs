using GymAppC.Application.DTOs.Workouts;
using MediatR;

namespace GymAppC.Application.Features.Workouts.Queries.GetWorkouts;

public sealed record GetWorkoutsQuery(int UserId) : IRequest<IReadOnlyList<WorkoutDto>>;
