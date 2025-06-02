using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.Login;
public class LoginUserCommandHandler(
    IUserService userService,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginUserCommand, AuthenticationResult>
{
    private readonly IUserService _userService = userService;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

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
            DateTime.UtcNow.AddDays(7));

        await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthenticationResult(userResponse, token, refreshToken.Value);
    }
}
