using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.RegisterUser;
public class RegisterUserCommandHandler(
    IUserService userService,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterUserCommand, AuthenticationResult>
{
    private readonly IUserService _userService = userService;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AuthenticationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userService.RegisterUserAsync(request, cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

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

        await _unitOfWork.SaveChangesAsync();

        return new AuthenticationResult(userResponse, token, refreshToken.Value);
    }
}
