using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.Login;
public class LoginUserCommandHandler(
    IUserService userService, 
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<LoginUserCommand, AuthenticationResult>
{
    private readonly IUserService _userService = userService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AuthenticationResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        User user = await _userService.LoginUserAsync(command.Email, command.Password, cancellationToken);

        string token = _jwtTokenGenerator.GenerateToken(user);

        var userResponse = new UserResponse(
            user.Id,
            user.Username,
            user.FullName,
            user.Email,
            user.MobileNumber,
            user.Language,
            user.Culture,
            user.IsAdmin);

        var refreshToken = new RefreshToken(
            _jwtTokenGenerator.GenerateRefreshToken(),
            user.Id,
            DateTime.UtcNow.AddHours(1));

        await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthenticationResult(userResponse, token, );
    }
}
