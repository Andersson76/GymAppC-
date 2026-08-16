using GymAppC.Application.Interfaces;
using GymAppC.Domain.Entities;

namespace GymAppC.Application.Tests.TestDoubles;

internal sealed class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = [];

    public bool EmailExists { get; set; }
    public bool TryAddSucceeds { get; set; } = true;
    public string? CheckedEmail { get; private set; }
    public User? AddedUser { get; private set; }
    public int TryAddCalls { get; private set; }
    public int SaveChangesCalls { get; private set; }

    public Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.SingleOrDefault(user => user.Email == email));
    }

    public Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        CheckedEmail = email;
        return Task.FromResult(EmailExists);
    }

    public Task<bool> TryAddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        TryAddCalls++;
        AddedUser = user;

        if (TryAddSucceeds)
        {
            _users.Add(user);
        }

        return Task.FromResult(TryAddSucceeds);
    }

    public Task<User?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.SingleOrDefault(user => user.Id == id));
    }

    public Task<IReadOnlyList<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<User>>(_users);
    }

    public Task AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        AddedUser = entity;
        _users.Add(entity);
        return Task.CompletedTask;
    }

    public void Update(User entity)
    {
    }

    public void Remove(User entity)
    {
        _users.Remove(entity);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesCalls++;
        return Task.FromResult(1);
    }
}
