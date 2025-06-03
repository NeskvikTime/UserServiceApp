using MediatR;
using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application.Users.LoginWithRefreshToken;

public record LoginWithRefreshTokenCommand(string RefreshToken) : IRequest<AuthenticationResult>;
