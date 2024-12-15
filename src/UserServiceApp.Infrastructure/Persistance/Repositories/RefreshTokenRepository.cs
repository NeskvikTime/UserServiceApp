using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain;

namespace UserServiceApp.Infrastructure.Persistance.Repositories;

internal class RefreshTokenRepository(ApplicationDbContext dbContext) : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }
}
