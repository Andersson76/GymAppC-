using FluentValidation;

namespace GymAppC.Application.Features.Workouts.Commands.UpdateWorkout;

public sealed class UpdateWorkoutCommandValidator : AbstractValidator<UpdateWorkoutCommand>
{
    public UpdateWorkoutCommandValidator()
    {
        RuleFor(command => command.Id)
            .GreaterThan(0);

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
