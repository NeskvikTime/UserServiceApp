using FluentValidation;

namespace UserServiceApp.Application.Users.UpdateUserData;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.FullName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage(errorMessage: "Password must have at least one number, symbol and capital letter. The minumum lenght is 8 characters,");

        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .Matches(@"^\+[1-9]\d{1,14}$")
            .WithMessage(errorMessage: "Mobile number must be in international format. Example: +40721234567");

        RuleFor(x => x.NewCulture)
            .NotEmpty()
            .Matches(@"^[A-Za-z]{1,8}(-[A-Za-z0-9]{1,8})*$")
            .WithMessage(errorMessage: "Culture should have correct format. Example: en-US");
    }
}
