using MediatR;
using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application.Users.Login;

public record LoginWithRefreshTokenCommand(string RefreshToken) : IRequest<AuthenticationResult>;
