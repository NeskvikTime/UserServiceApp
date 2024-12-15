using UserServiceApp.Domain;

namespace UserServiceApp.Application.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}