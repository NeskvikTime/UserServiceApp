using MediatR;
using UserServiceApp.Application.Common.Interfaces;

namespace UserServiceApp.Application.Users.DeleteUser;
public class DeleteUserCommandHandler(IUserService _userService) : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userService.DeleteUeserAsync(request.UserId, cancellationToken);
        return Unit.Value;
    }
}
