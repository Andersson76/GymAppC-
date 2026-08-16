using MediatR;

namespace GymAppC.Application.Features.Workouts.Commands.UpdateWorkout;

public sealed record UpdateWorkoutCommand(
    int Id,
    string Title,
    DateTime Date,
    string? Notes,
    int UserId) : IRequest<bool>;
