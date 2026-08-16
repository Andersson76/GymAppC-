using GymAppC.Application.Interfaces;
using GymAppC.Domain.Entities;
using GymAppC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymAppC.Infrastructure.Repositories;

public sealed class WorkoutRepository : Repository<Workout>, IWorkoutRepository
{
    public WorkoutRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Workout>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await Entities
            .AsNoTracking()
            .Where(workout => workout.UserId == userId)
            .OrderByDescending(workout => workout.Date)
            .ToListAsync(cancellationToken);
    }

    public Task<Workout?> GetByIdForUserAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return Entities.FirstOrDefaultAsync(
            workout => workout.Id == id && workout.UserId == userId,
            cancellationToken);
    }
}
