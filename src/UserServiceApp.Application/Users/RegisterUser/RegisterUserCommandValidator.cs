using FluentValidation;

namespace UserServiceApp.Application.Users.RegisterUser;
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .Matches(@"^\+[1-9]\d{1,14}$")
            .WithMessage(errorMessage: "Mobile number must be in international format. Example: +40721234567");

        RuleFor(x => x.Language)
            .NotEmpty()
            .Matches(@"^[a-z]{2}-[A-Z]{2}$")
            .WithMessage(errorMessage: "Language must be in format: en-US");

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage(errorMessage: "Password must have at least one number, symbol and capital letter. The minumum lenght is 8 characters,");

    }
}
