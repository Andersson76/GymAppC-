using FluentValidation;

namespace GymAppC.Application.Features.Auth.Queries.Login;

public sealed class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {
        RuleFor(query => query.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(query => query.Password)
            .NotEmpty()
            .MaximumLength(128);
    }
}
