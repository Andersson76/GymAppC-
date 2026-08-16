using AutoMapper;
using GymAppC.Application.DTOs.Workouts;
using GymAppC.Application.Interfaces;
using GymAppC.Domain.Entities;
using MediatR;

namespace GymAppC.Application.Features.Workouts.Commands.CreateWorkout;

public sealed class CreateWorkoutCommandHandler
    : IRequestHandler<CreateWorkoutCommand, WorkoutDto>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IMapper _mapper;

    public CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository, IMapper mapper)
    {
        _workoutRepository = workoutRepository;
        _mapper = mapper;
    }

    public async Task<WorkoutDto> Handle(
        CreateWorkoutCommand request,
        CancellationToken cancellationToken)
    {
        var workout = _mapper.Map<Workout>(request);

        await _workoutRepository.AddAsync(workout, cancellationToken);
        await _workoutRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkoutDto>(workout);
    }
}
