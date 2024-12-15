using MediatR;

using UserServiceApp.Application.Common.Interfaces;

namespace UserServiceApp.Application.Users;
internal class LoginUserWithRefreshToken(IRefreshTokenRepository refreshTokenRepository, IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;


}
