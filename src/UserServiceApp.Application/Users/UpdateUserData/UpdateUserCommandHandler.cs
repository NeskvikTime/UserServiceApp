using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.UpdateUserData;
public class UpdateUserCommandHandler(IUserService userService)
    : IRequestHandler<UpdateUserCommand, UserResponse>
{
    private readonly IUserService _userService = userService;

    public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userService.UpdateUserAsync(request, cancellationToken);

        return new UserResponse(
            user.Id,
            user.Username,
            user.FullName,
            user.Email,
            user.MobileNumber,
            user.Language,
            user.Culture,
            user.IsAdmin);
    }
}
