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
            .MustAsync(ValidateUserExistsAsync)
            .WithMessage(x => $"User with Id: {x.UserId} does not exist.");
    }

    private async Task<bool> ValidateUserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(userId, cancellationToken);
        return user != null;
    }
}
