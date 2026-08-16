using FluentValidation;

namespace GymAppC.Application.Features.Users.Queries.GetCurrentUser;

public sealed class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
{
    public GetCurrentUserQueryValidator()
    {
        RuleFor(query => query.UserId)
            .GreaterThan(0);
    }
}
