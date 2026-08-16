using GymAppC.Application.Interfaces;
using MediatR;

namespace GymAppC.Application.Features.Workouts.Commands.DeleteWorkout;

public sealed class DeleteWorkoutCommandHandler
    : IRequestHandler<DeleteWorkoutCommand, bool>
{
    private readonly IWorkoutRepository _workoutRepository;

    public DeleteWorkoutCommandHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<bool> Handle(
        DeleteWorkoutCommand request,
        CancellationToken cancellationToken)
    {
        var workout = await _workoutRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (workout is null)
        {
            return false;
        }

        _workoutRepository.Remove(workout);
        await _workoutRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
