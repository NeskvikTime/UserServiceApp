using MediatR;
using UserServiceApp.Application.Common.Authorization;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.Application.Users.GetUserDatas;

[Authorize(Roles = "Admin")]
public record GetUserDatasQuery(Guid? UserId) : IRequest<List<UserResponse>>;