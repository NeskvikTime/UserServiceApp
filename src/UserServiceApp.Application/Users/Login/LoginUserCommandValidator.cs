using FluentValidation;
using UserServiceApp.Application.Common.Interfaces;

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
            .MustAsync(ValidateUserExistsByEmailAsync);

        RuleFor(x => x.Password)
            .NotEmpty();
    }

    private async Task<bool> ValidateUserExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByEmailAsync(email, cancellationToken);

        return user != null;
    }
}
