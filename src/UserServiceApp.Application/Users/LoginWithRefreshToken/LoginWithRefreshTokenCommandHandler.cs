using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.Common.Interfaces;

namespace UserServiceApp.Application.Users.LoginWithRefreshToken;

public class LoginWithRefreshTokenCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<LoginWithRefreshTokenCommand, AuthenticationResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

    public async Task<AuthenticationResult> Handle(LoginWithRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetRefreshTokenWithUserAsync(request.RefreshToken, cancellationToken);

        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("The refresh token is invalid or has expired.");
        }

        var user = refreshToken.User;

        if (user is null)
        {
            throw new UnauthorizedAccessException("User not found for this refresh token.");
        }

        string accessToken = _jwtTokenGenerator.GenerateToken(user);
        string newRefreshTokenValue = _jwtTokenGenerator.GenerateRefreshToken();
        refreshToken.UpdateValueAndExpiration(newRefreshTokenValue, DateTime.UtcNow.AddDays(7));

        await _refreshTokenRepository.UpdateRefreshTokenAsync(refreshToken, cancellationToken);

        var userResponse = new UserResponse(
            user.Id,
            user.Username,
            user.FullName,
            user.Email,
            user.MobileNumber,
            user.Language,
            user.Culture,
            user.IsAdmin);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthenticationResult(userResponse, accessToken, newRefreshTokenValue);
    }
}
