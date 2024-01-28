using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.GetUserDatas;
public class GetAllUserDatasQueryHandler(IUserService _userService) : IRequestHandler<GetAllUserDatasQuery, List<UserResponse>>
{
    public async Task<List<UserResponse>> Handle(GetAllUserDatasQuery request, CancellationToken cancellationToken)
    {
        List<User> users = await _userService.GetAllUserDatasAsync(cancellationToken);

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
