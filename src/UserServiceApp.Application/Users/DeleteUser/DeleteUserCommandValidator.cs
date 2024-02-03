using FluentValidation;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain.Exceptions;

namespace UserServiceApp.Application.Users.DeleteUser;
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    private readonly IUsersRepository _usersRepository;

    public DeleteUserCommandValidator(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .CustomAsync(ValidateUserExistsAsync);
    }

    private async Task ValidateUserExistsAsync(Guid userId,
        ValidationContext<DeleteUserCommand> context,
        CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException($"User with Id: '{userId}' has not been found.");
        }
    }
}
