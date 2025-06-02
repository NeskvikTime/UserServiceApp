using Microsoft.EntityFrameworkCore;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Infrastructure.Persistence.Repositories;

internal class RefreshTokenRepository(ApplicationDbContext dbContext) : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenWithUserAsync(string value, CancellationToken cancellationToken)
    {
        return await _dbContext.RefreshTokens
            .Include(r => r.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Value == value, cancellationToken);
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        _dbContext.RefreshTokens.Update(refreshToken);
    }
}
