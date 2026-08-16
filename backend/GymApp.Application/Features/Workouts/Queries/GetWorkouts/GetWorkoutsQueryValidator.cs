using FluentValidation;

namespace GymAppC.Application.Features.Workouts.Queries.GetWorkouts;

public sealed class GetWorkoutsQueryValidator : AbstractValidator<GetWorkoutsQuery>
{
    public GetWorkoutsQueryValidator()
    {
        RuleFor(query => query.UserId)
            .GreaterThan(0);
    }
}
