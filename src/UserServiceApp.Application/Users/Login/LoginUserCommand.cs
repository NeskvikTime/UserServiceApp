using MediatR;
using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application.Users.Login;

public record LoginUserCommand(
    string Email,
    string Password) : IRequest<AuthenticationResult>;
