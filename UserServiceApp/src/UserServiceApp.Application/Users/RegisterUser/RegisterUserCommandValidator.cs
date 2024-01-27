using FluentValidation;

namespace UserServiceApp.Application.Users.RegisterUser;
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {

    }
}
