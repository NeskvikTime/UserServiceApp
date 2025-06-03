using FluentValidation;

namespace UserServiceApp.Application.Users.LoginWithRefreshToken;
public class LoginWithRefreshTokenCommandValidator : AbstractValidator<LoginWithRefreshTokenCommand>
{
    public LoginWithRefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token must not be empty.");

        RuleFor(x => x.RefreshToken)
            .Length(44)
            .WithMessage("Refresh token must be exactly 44 characters long.");
    }
}
