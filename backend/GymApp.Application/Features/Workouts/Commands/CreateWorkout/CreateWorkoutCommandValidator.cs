using FluentValidation;

namespace GymAppC.Application.Features.Workouts.Commands.CreateWorkout;

public sealed class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
{
    public CreateWorkoutCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Date)
            .NotEmpty();

        RuleFor(command => command.Notes)
            .MaximumLength(500);

        RuleFor(command => command.UserId)
            .GreaterThan(0);
    }
}
