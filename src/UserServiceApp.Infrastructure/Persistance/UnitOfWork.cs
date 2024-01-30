using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using UserServiceApp.Application.Common.Interfaces;

namespace UserServiceApp.Infrastructure.Persistance;
internal class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(ApplicationDbContext dbContext, ILogger<UnitOfWork> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    private bool _isUnitOfWorkStarted = false;

    public bool IsUnitOfWorkStarted => _isUnitOfWorkStarted;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void StartUnitOfWork()
    {
        _isUnitOfWorkStarted = true;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default)
    {
        try
        {
            _logger.LogDebug("Starting transaction for {DbContext}.", nameof(UnitOfWork));
            return await _dbContext.Database.BeginTransactionAsync(token).ConfigureAwait(false);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
