using AutoMapper;
using GymAppC.Application.DTOs.Workouts;
using GymAppC.Application.Interfaces;
using MediatR;

namespace GymAppC.Application.Features.Workouts.Queries.GetWorkouts;

public sealed class GetWorkoutsQueryHandler
    : IRequestHandler<GetWorkoutsQuery, IReadOnlyList<WorkoutDto>>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IMapper _mapper;

    public GetWorkoutsQueryHandler(IWorkoutRepository workoutRepository, IMapper mapper)
    {
        _workoutRepository = workoutRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<WorkoutDto>> Handle(
        GetWorkoutsQuery request,
        CancellationToken cancellationToken)
    {
        var workouts = await _workoutRepository.GetAllByUserIdAsync(
            request.UserId,
            cancellationToken);

        return _mapper.Map<IReadOnlyList<WorkoutDto>>(workouts);
    }
}
