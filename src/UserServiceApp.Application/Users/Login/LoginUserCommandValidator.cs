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
            .EmailAddress()
            .CustomAsync(ValidateUserExistsByEmailAsync);

        RuleFor(x => x.Password)
            .NotEmpty();
    }

    private async Task ValidateUserExistsByEmailAsync(string email,
        ValidationContext<LoginUserCommand> context,
        CancellationToken cancellationToken)
    {
        bool exsists = await _usersRepository.UserByEmailExistsAsync(email, cancellationToken);

        if (exsists)
        {
            throw new AuthorizationException("Wrong username or password.");
        }
    }
}
