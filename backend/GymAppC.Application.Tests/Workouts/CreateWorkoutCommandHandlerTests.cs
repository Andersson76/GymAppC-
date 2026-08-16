using AutoMapper;
using GymAppC.Application.Features.Workouts.Commands.CreateWorkout;
using GymAppC.Application.Mappings;
using GymAppC.Application.Tests.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;

namespace GymAppC.Application.Tests.Workouts;

public sealed class CreateWorkoutCommandHandlerTests
{
    [Fact]
    public async Task Handle_MapsPersistsAndReturnsCreatedWorkout()
    {
        var repository = new FakeWorkoutRepository { AssignedId = 73 };
        var handler = new CreateWorkoutCommandHandler(repository, CreateMapper());
        var date = new DateTime(2026, 8, 16, 10, 30, 0, DateTimeKind.Utc);
        var command = new CreateWorkoutCommand("Strength", date, "Heavy day", 9);

        var result = await handler.Handle(command, CancellationToken.None);

        var workout = Assert.IsType<GymAppC.Domain.Entities.Workout>(repository.AddedWorkout);
        Assert.Equal(73, workout.Id);
        Assert.Equal("Strength", workout.Title);
        Assert.Equal(date, workout.Date);
        Assert.Equal("Heavy day", workout.Notes);
        Assert.Equal(9, workout.UserId);
        Assert.Equal(1, repository.SaveChangesCalls);

        Assert.Equal(workout.Id, result.Id);
        Assert.Equal(workout.Title, result.Title);
        Assert.Equal(workout.Date, result.Date);
        Assert.Equal(workout.Notes, result.Notes);
        Assert.Equal(workout.UserId, result.UserId);
    }

    private static IMapper CreateMapper()
    {
        var configuration = new MapperConfiguration(
            expression => expression.AddProfile<ApplicationMappingProfile>(),
            NullLoggerFactory.Instance);

        return configuration.CreateMapper();
    }
}
