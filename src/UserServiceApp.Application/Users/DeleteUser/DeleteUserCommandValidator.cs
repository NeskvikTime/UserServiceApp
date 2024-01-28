using FluentValidation;
using UserServiceApp.Application.Common.Interfaces;

namespace UserServiceApp.Application.Users.DeleteUser;
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    private readonly IUsersRepository _usersRepository;

    public DeleteUserCommandValidator(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await _usersRepository.GetByIdAsync(userId, cancellationToken);

                return user != null;
            })
            .WithMessage(x => $"User with id: {x.UserId} does not exist.");
    }
}
