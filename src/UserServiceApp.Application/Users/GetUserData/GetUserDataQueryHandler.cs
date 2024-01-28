using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application.Users.GetUserData;
public class GetUserDataQueryHandler(IUserService _userService) : IRequestHandler<GetUserDataQuery, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(GetUserDataQuery request, CancellationToken cancellationToken)
    {
        AuthenticationResult result = await _userService.GetUserDataAsync(request.UserId, cancellationToken);

        return result;
    }
}
