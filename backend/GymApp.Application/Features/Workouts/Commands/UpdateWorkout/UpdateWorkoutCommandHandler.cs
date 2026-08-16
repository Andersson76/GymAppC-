using AutoMapper;
using GymAppC.Application.Interfaces;
using MediatR;

namespace GymAppC.Application.Features.Workouts.Commands.UpdateWorkout;

public sealed class UpdateWorkoutCommandHandler
    : IRequestHandler<UpdateWorkoutCommand, bool>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IMapper _mapper;

    public UpdateWorkoutCommandHandler(IWorkoutRepository workoutRepository, IMapper mapper)
    {
        _workoutRepository = workoutRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(
        UpdateWorkoutCommand request,
        CancellationToken cancellationToken)
    {
        var workout = await _workoutRepository.GetByIdForUserAsync(
            request.Id,
            request.UserId,
            cancellationToken);

        if (workout is null)
        {
            return false;
        }

        _mapper.Map(request, workout);
        _workoutRepository.Update(workout);
        await _workoutRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
