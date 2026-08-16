using AutoMapper;
using GymAppC.Application.DTOs.Workouts;
using GymAppC.Application.Interfaces;
using MediatR;

namespace GymAppC.Application.Features.Workouts.Queries.GetWorkoutById;

public sealed class GetWorkoutByIdQueryHandler
    : IRequestHandler<GetWorkoutByIdQuery, WorkoutDto?>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IMapper _mapper;

    public GetWorkoutByIdQueryHandler(IWorkoutRepository workoutRepository, IMapper mapper)
    {
        _workoutRepository = workoutRepository;
        _mapper = mapper;
    }

    public async Task<WorkoutDto?> Handle(
        GetWorkoutByIdQuery request,
        CancellationToken cancellationToken)
    {
        var workout = await _workoutRepository.GetByIdForUserAsync(
            request.Id,
            request.UserId,
            cancellationToken);

        return _mapper.Map<WorkoutDto?>(workout);
    }
}
