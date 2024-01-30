using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UserServiceApp.Domain.Common;
using UserServiceApp.Domain.Common.Interfaces;

namespace UserServiceApp.Infrastructure.Persistance;

public class SaveChangesInterceptor(
    IDateTimeProvider _dateTimeProvider) : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var _context = eventData.Context as ApplicationDbContext;

        if (_context is null)
        {
            throw new ArgumentNullException(nameof(ApplicationDbContext));
        }

        DateTime savingDateTime = _dateTimeProvider.UtcNow;

        foreach (var entry in _context.ChangeTracker.Entries<BaseEntity>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.DateModified = savingDateTime;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = savingDateTime;
            }
        }

        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return ValueTask.FromResult(SavingChanges(eventData, result));
    }

}