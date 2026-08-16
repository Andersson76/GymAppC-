using FluentValidation;

namespace GymAppC.Application.Features.Workouts.Queries.GetWorkoutById;

public sealed class GetWorkoutByIdQueryValidator : AbstractValidator<GetWorkoutByIdQuery>
{
    public GetWorkoutByIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .GreaterThan(0);

        RuleFor(query => query.UserId)
            .GreaterThan(0);
    }
}
