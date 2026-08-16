using FluentValidation;

namespace GymAppC.Application.Features.Workouts.Commands.DeleteWorkout;

public sealed class DeleteWorkoutCommandValidator : AbstractValidator<DeleteWorkoutCommand>
{
    public DeleteWorkoutCommandValidator()
    {
        RuleFor(command => command.Id)
            .GreaterThan(0);
    }
}
