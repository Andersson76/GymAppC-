using GymAppC.Application.DTOs.Workouts;
using MediatR;

namespace GymAppC.Application.Features.Workouts.Commands.CreateWorkout;

public sealed record CreateWorkoutCommand(
    string Title,
    DateTime Date,
    string? Notes,
    int UserId) : IRequest<WorkoutDto>;
