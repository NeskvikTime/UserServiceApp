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
            .MustAsync(UsernameIsUniqueAsync)
            .WithMessage("Username is already taken.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(EmailIsUniqueAsync)
            .WithMessage("Email is already taken.");

        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .Matches(@"^\+[1-9]\d{1,14}$")
            .WithMessage("Mobile number must be in international format. Example: +40721234567");

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage("Password must have at least one number, symbol and capital letter. The minumum lenght is 8 characters.");
    }

    private async Task<bool> UsernameIsUniqueAsync(string username, CancellationToken cancellationToken)
    {
        var usernameIsUnique = await _usersRepository.UsernameIsUniqueAsync(username, cancellationToken);
        return usernameIsUnique;
    }

    private async Task<bool> EmailIsUniqueAsync(string email, CancellationToken cancellationToken)
    {
        bool emailIsUnique = await _usersRepository.EmailIsUniqueAsync(email, cancellationToken);
        return emailIsUnique;
    }
}
