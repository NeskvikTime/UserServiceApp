using FluentValidation;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain.Exceptions;

namespace UserServiceApp.Application.Users.Login;
public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    private readonly IUsersRepository _usersRepository;

    public LoginUserCommandValidator(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress();

        When(x => !string.IsNullOrWhiteSpace(x.Email) && !string.IsNullOrWhiteSpace(x.Password), () =>
        {
            RuleFor(x => x.Email)
                .CustomAsync(ValidateUserExistsByEmailAsync);
        });

        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull();
    }

    private async Task ValidateUserExistsByEmailAsync(string email, ValidationContext<LoginUserCommand> context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
        {
            return;
        }

        bool exsists = await _usersRepository.UserByEmailExistsAsync(email, cancellationToken);

        if (!exsists)
        {
            throw new AuthorizationException("Wrong username or password.");
        }
    }
}
