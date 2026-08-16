using GymAppC.Application.Features.Workouts.Commands.DeleteWorkout;
using GymAppC.Application.Tests.TestDoubles;
using GymAppC.Domain.Entities;

namespace GymAppC.Application.Tests.Workouts;

public sealed class DeleteWorkoutCommandHandlerTests
{
    [Fact]
    public async Task Handle_AdminFlowDeletesWorkoutRegardlessOfOwner()
    {
        var repository = new FakeWorkoutRepository();
        repository.Seed(new Workout
        {
            Id = 17,
            Title = "Owned by another user",
            Date = DateTime.UtcNow,
            UserId = 99
        });
        var handler = new DeleteWorkoutCommandHandler(repository);

        var deleted = await handler.Handle(
            new DeleteWorkoutCommand(17),
            CancellationToken.None);

        Assert.True(deleted);
        Assert.Null(await repository.GetByIdAsync(17));
        Assert.Equal(1, repository.SaveChangesCalls);
    }

    [Fact]
    public async Task Handle_MissingWorkoutReturnsFalseWithoutSaving()
    {
        var repository = new FakeWorkoutRepository();
        var handler = new DeleteWorkoutCommandHandler(repository);

        var deleted = await handler.Handle(
            new DeleteWorkoutCommand(404),
            CancellationToken.None);

        Assert.False(deleted);
        Assert.Equal(0, repository.SaveChangesCalls);
    }
}
