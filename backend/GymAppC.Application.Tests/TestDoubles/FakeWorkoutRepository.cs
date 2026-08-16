using GymAppC.Application.Interfaces;
using GymAppC.Domain.Entities;

namespace GymAppC.Application.Tests.TestDoubles;

internal sealed class FakeWorkoutRepository : IWorkoutRepository
{
    private readonly List<Workout> _workouts = [];

    public int AssignedId { get; init; } = 42;
    public Workout? AddedWorkout { get; private set; }
    public Workout? UpdatedWorkout { get; private set; }
    public int SaveChangesCalls { get; private set; }

    public void Seed(Workout workout)
    {
        _workouts.Add(workout);
    }

    public Task<Workout?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_workouts.SingleOrDefault(workout => workout.Id == id));
    }

    public Task<IReadOnlyList<Workout>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Workout>>(_workouts);
    }

    public Task<IReadOnlyList<Workout>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Workout> workouts = _workouts
            .Where(workout => workout.UserId == userId)
            .ToArray();

        return Task.FromResult(workouts);
    }

    public Task<Workout?> GetByIdForUserAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            _workouts.SingleOrDefault(workout => workout.Id == id && workout.UserId == userId));
    }

    public Task AddAsync(Workout entity, CancellationToken cancellationToken = default)
    {
        entity.Id = AssignedId;
        AddedWorkout = entity;
        _workouts.Add(entity);
        return Task.CompletedTask;
    }

    public void Update(Workout entity)
    {
        UpdatedWorkout = entity;
    }

    public void Remove(Workout entity)
    {
        _workouts.Remove(entity);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesCalls++;
        return Task.FromResult(1);
    }
}
