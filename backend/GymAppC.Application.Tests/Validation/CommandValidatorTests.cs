using GymAppC.Application.Features.Auth.Commands.Register;
using GymAppC.Application.Features.Workouts.Commands.CreateWorkout;

namespace GymAppC.Application.Tests.Validation;

public sealed class CommandValidatorTests
{
    [Fact]
    public void RegisterValidator_RejectsMissingNameInvalidEmailAndShortPassword()
    {
        var validator = new RegisterUserCommandValidator();

        var result = validator.Validate(
            new RegisterUserCommand(string.Empty, "not-an-email", "12345"));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(RegisterUserCommand.Name));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(RegisterUserCommand.Email));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(RegisterUserCommand.Password));
    }

    [Fact]
    public void RegisterValidator_AcceptsValidCommand()
    {
        var validator = new RegisterUserCommandValidator();

        var result = validator.Validate(
            new RegisterUserCommand("Ada", "ada@example.com", "secret1"));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateWorkoutValidator_RejectsInvalidFields()
    {
        var validator = new CreateWorkoutCommandValidator();

        var result = validator.Validate(
            new CreateWorkoutCommand(string.Empty, default, new string('n', 501), 0));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateWorkoutCommand.Title));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateWorkoutCommand.Date));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateWorkoutCommand.Notes));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateWorkoutCommand.UserId));
    }

    [Fact]
    public void CreateWorkoutValidator_AcceptsValidCommand()
    {
        var validator = new CreateWorkoutCommandValidator();

        var result = validator.Validate(
            new CreateWorkoutCommand("Cardio", DateTime.UtcNow, null, 5));

        Assert.True(result.IsValid);
    }
}
