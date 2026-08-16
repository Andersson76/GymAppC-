using GymAppC.Domain.Entities;

namespace GymAppC.Application.Interfaces;

public interface IWorkoutRepository : IRepository<Workout>
{
    Task<IReadOnlyList<Workout>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<Workout?> GetByIdForUserAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);
}
