using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);

    string GenerateRefreshToken();
}