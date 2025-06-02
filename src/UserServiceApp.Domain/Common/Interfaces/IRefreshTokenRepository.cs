using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Domain.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<RefreshToken?> GetRefreshTokenWithUserAsync(string value, CancellationToken cancellationToken);
    Task UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}