using Microsoft.EntityFrameworkCore.Storage;

namespace UserServiceApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    void StartUnitOfWork();

    bool IsUnitOfWorkStarted { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default);
}