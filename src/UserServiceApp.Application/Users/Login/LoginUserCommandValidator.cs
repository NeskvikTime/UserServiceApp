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
            .MustAsync(async (email, cancellationToken) =>
            {
                var user = await usersRepository.GetUserByEmailAsync(email, cancellationToken);

                return user != null;
            });

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
