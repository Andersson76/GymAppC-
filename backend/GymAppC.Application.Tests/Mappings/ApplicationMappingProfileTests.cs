using AutoMapper;
using GymAppC.Application.Dtos;
using GymAppC.Application.DTOs.Workouts;
using GymAppC.Application.Features.Workouts.Commands.CreateWorkout;
using GymAppC.Application.Features.Workouts.Commands.UpdateWorkout;
using GymAppC.Application.Mappings;
using GymAppC.Domain.Constants;
using GymAppC.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace GymAppC.Application.Tests.Mappings;

public sealed class ApplicationMappingProfileTests
{
    [Fact]
    public void Configuration_IsValid()
    {
        var configuration = CreateConfiguration();

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void UserMappings_CopyPublicFieldsWithoutCreatingToken()
    {
        var mapper = CreateConfiguration().CreateMapper();
        var user = new User
        {
            Id = 12,
            Name = "Ada Lovelace",
            Email = "ada@example.com",
            Role = AppRoles.Admin,
            PasswordHash = [1],
            PasswordSalt = [2]
        };

        var authResponse = mapper.Map<AuthResponseDto>(user);
        var currentUser = mapper.Map<CurrentUserDto>(user);

        Assert.Equal(user.Name, authResponse.Name);
        Assert.Equal(user.Email, authResponse.Email);
        Assert.Equal(user.Role, authResponse.Role);
        Assert.Equal(string.Empty, authResponse.Token);

        Assert.Equal(user.Id, currentUser.Id);
        Assert.Equal(user.Name, currentUser.Name);
        Assert.Equal(user.Email, currentUser.Email);
        Assert.Equal(user.Role, currentUser.Role);
    }

    [Fact]
    public void WorkoutMappings_MapCreateAndDto_AndPreserveOwnershipOnUpdate()
    {
        var mapper = CreateConfiguration().CreateMapper();
        var createdAt = new DateTime(2026, 8, 16, 9, 0, 0, DateTimeKind.Utc);
        var createCommand = new CreateWorkoutCommand(
            "Strength",
            createdAt,
            "Squats",
            21);

        var workout = mapper.Map<Workout>(createCommand);

        Assert.Equal(createCommand.Title, workout.Title);
        Assert.Equal(createCommand.Date, workout.Date);
        Assert.Equal(createCommand.Notes, workout.Notes);
        Assert.Equal(createCommand.UserId, workout.UserId);
        Assert.Equal(0, workout.Id);
        Assert.Empty(workout.Exercises);

        workout.Id = 44;
        var dto = mapper.Map<WorkoutDto>(workout);

        Assert.Equal(workout.Id, dto.Id);
        Assert.Equal(workout.Title, dto.Title);
        Assert.Equal(workout.Date, dto.Date);
        Assert.Equal(workout.Notes, dto.Notes);
        Assert.Equal(workout.UserId, dto.UserId);

        var owner = new User { Id = workout.UserId };
        var exercises = new List<Exercise>
        {
            new() { Id = 3, Name = "Squat", Sets = 3, Reps = 5 }
        };
        workout.User = owner;
        workout.Exercises = exercises;
        var updatedAt = createdAt.AddDays(1);
        var updateCommand = new UpdateWorkoutCommand(
            999,
            "Updated strength",
            updatedAt,
            "Deadlifts",
            999);

        mapper.Map(updateCommand, workout);

        Assert.Equal(44, workout.Id);
        Assert.Equal(21, workout.UserId);
        Assert.Same(owner, workout.User);
        Assert.Same(exercises, workout.Exercises);
        Assert.Equal(updateCommand.Title, workout.Title);
        Assert.Equal(updateCommand.Date, workout.Date);
        Assert.Equal(updateCommand.Notes, workout.Notes);
    }

    private static MapperConfiguration CreateConfiguration()
    {
        return new MapperConfiguration(
            expression => expression.AddProfile<ApplicationMappingProfile>(),
            NullLoggerFactory.Instance);
    }
}
