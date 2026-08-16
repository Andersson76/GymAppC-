using GymAppC.Application.DTOs.Workouts;
using MediatR;

namespace GymAppC.Application.Features.Workouts.Queries.GetWorkoutById;

public sealed record GetWorkoutByIdQuery(int Id, int UserId) : IRequest<WorkoutDto?>;
