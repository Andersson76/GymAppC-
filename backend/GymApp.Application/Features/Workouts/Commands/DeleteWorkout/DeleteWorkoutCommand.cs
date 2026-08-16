using MediatR;

namespace GymAppC.Application.Features.Workouts.Commands.DeleteWorkout;

public sealed record DeleteWorkoutCommand(int Id) : IRequest<bool>;
