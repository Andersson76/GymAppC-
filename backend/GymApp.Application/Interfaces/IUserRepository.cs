using GymAppC.Domain.Entities;

namespace GymAppC.Application.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> TryAddAsync(User user, CancellationToken cancellationToken = default);
}
