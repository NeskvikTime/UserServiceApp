using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.GetUserDatas;
public class GetUserDatasQueryHandler(IUserService _userService) : IRequestHandler<GetUserDataQuery, List<UserResponse>>
{
    public async Task<List<UserResponse>> Handle(GetUserDataQuery request, CancellationToken cancellationToken)
    {
        List<User> users = new List<User>();

        users = await _userService.GetUsersAsync(request.UserId, cancellationToken);

        var response = new List<UserResponse>();

        users.ForEach(u => response.Add(new UserResponse(
            u.Id,
            u.Username,
            u.FullName,
            u.Email,
            u.MobileNumber,
            u.Language,
            u.Culture,
            u.IsAdmin)));

        return response;
    }
}
