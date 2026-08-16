using GymAppC.Application.Features.Auth.Queries.Login;
using GymAppC.Application.Features.Users.Queries.GetCurrentUser;
using GymAppC.Application.Features.Workouts.Commands.DeleteWorkout;
using GymAppC.Application.Features.Workouts.Commands.UpdateWorkout;
using GymAppC.Application.Features.Workouts.Queries.GetWorkoutById;
using GymAppC.Application.Features.Workouts.Queries.GetWorkouts;

namespace GymAppC.Application.Tests.Validation;

public sealed class AdditionalValidatorTests
{
    [Fact]
    public void LoginValidator_RejectsInvalidEmailAndMissingPassword()
    {
        var validator = new LoginUserQueryValidator();

        var result = validator.Validate(new LoginUserQuery("not-an-email", string.Empty));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(LoginUserQuery.Email));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(LoginUserQuery.Password));
    }

    [Fact]
    public void LoginValidator_AcceptsValidQuery()
    {
        var validator = new LoginUserQueryValidator();

        var result = validator.Validate(new LoginUserQuery("ada@example.com", "secret1"));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CurrentUserValidator_RejectsInvalidUserId()
    {
        var validator = new GetCurrentUserQueryValidator();

        var result = validator.Validate(new GetCurrentUserQuery(0));

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(GetCurrentUserQuery.UserId));
    }

    [Fact]
    public void CurrentUserValidator_AcceptsValidQuery()
    {
        var validator = new GetCurrentUserQueryValidator();

        var result = validator.Validate(new GetCurrentUserQuery(7));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void UpdateWorkoutValidator_RejectsAllInvalidFields()
    {
        var validator = new UpdateWorkoutCommandValidator();
        var command = new UpdateWorkoutCommand(
            0,
            string.Empty,
            default,
            new string('n', 501),
            0);

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateWorkoutCommand.Id));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateWorkoutCommand.Title));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateWorkoutCommand.Date));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateWorkoutCommand.Notes));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateWorkoutCommand.UserId));
    }

    [Fact]
    public void UpdateWorkoutValidator_AcceptsValidCommand()
    {
        var validator = new UpdateWorkoutCommandValidator();
        var command = new UpdateWorkoutCommand(
            4,
            "Strength",
            new DateTime(2026, 8, 16, 9, 0, 0, DateTimeKind.Utc),
            null,
            7);

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void DeleteWorkoutValidator_RejectsInvalidId()
    {
        var validator = new DeleteWorkoutCommandValidator();

        var result = validator.Validate(new DeleteWorkoutCommand(0));

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(DeleteWorkoutCommand.Id));
    }

    [Fact]
    public void DeleteWorkoutValidator_AcceptsValidCommand()
    {
        var validator = new DeleteWorkoutCommandValidator();

        var result = validator.Validate(new DeleteWorkoutCommand(4));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void GetWorkoutByIdValidator_RejectsInvalidIds()
    {
        var validator = new GetWorkoutByIdQueryValidator();

        var result = validator.Validate(new GetWorkoutByIdQuery(0, 0));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetWorkoutByIdQuery.Id));
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(GetWorkoutByIdQuery.UserId));
    }

    [Fact]
    public void GetWorkoutByIdValidator_AcceptsValidQuery()
    {
        var validator = new GetWorkoutByIdQueryValidator();

        var result = validator.Validate(new GetWorkoutByIdQuery(4, 7));

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(7, true)]
    public void GetWorkoutsValidator_ValidatesUserId(int userId, bool expectedIsValid)
    {
        var validator = new GetWorkoutsQueryValidator();

        var result = validator.Validate(new GetWorkoutsQuery(userId));

        Assert.Equal(expectedIsValid, result.IsValid);
    }
}
