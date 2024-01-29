using FluentValidation;
using UserServiceApp.Application.Common.Interfaces;

namespace UserServiceApp.Application.Users.RegisterUser;
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly IUsersRepository _usersRepository;

    public RegisterUserCommandValidator(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(async (username, cancellationToken) =>
            {
                var usernameIsUnique = await _usersRepository.UsernameIsUniqueAsync(username, cancellationToken);

                return usernameIsUnique;
            })
            .WithMessage(errorMessage: "Username already exists");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, cancellationToken) =>
            {
                var emailIsUnique = await _usersRepository.EmailIsUniqueAsync(email, cancellationToken);

                return emailIsUnique;
            })
            .WithMessage(errorMessage: "Email already exists");

        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .Matches(@"^\+[1-9]\d{1,14}$")
            .WithMessage(errorMessage: "Mobile number must be in international format. Example: +40721234567");

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage(errorMessage: "Password must have at least one number, symbol and capital letter. The minumum lenght is 8 characters,");

    }
}
