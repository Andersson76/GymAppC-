using GymAppC.Application.Interfaces;
using GymAppC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymAppC.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected AppDbContext Context { get; }
    protected DbSet<T> Entities => Context.Set<T>();

    public Repository(AppDbContext context)
    {
        Context = context;
    }

    public virtual async Task<T?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await Entities.FindAsync([id], cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await Entities.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        await Entities.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        Entities.Update(entity);
    }

    public virtual void Remove(T entity)
    {
        Entities.Remove(entity);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }
}
