using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UserServiceApp.Domain.Common;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Infrastructure.Common;

namespace UserServiceApp.Infrastructure.Persistance;

public class SaveChangesInterceptor(
    IHttpContextAccessor _httpContextAccessor,
    IPublisher _publisher,
    IDateTimeProvider _dateTimeProvider) : ISaveChangesInterceptor
{
    private bool IsUserWaitingOnline() => _httpContextAccessor.HttpContext is not null;

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

        var domainEvents = _context.ChangeTracker.Entries<AggregateRoot>()
           .Select(entry => entry.Entity.PopDomainEvents())
           .SelectMany(x => x)
           .ToList();

        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
        }

        ValueTask.FromResult(PublishDomainEvents(domainEvents));

        return result;
    }

    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return ValueTask.FromResult(SavingChanges(eventData, result));
    }

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        Queue<IDomainEvent> domainEventsQueue = _httpContextAccessor.HttpContext!.Items.TryGetValue(EventualConsistencyMiddleware.DomainEventsKey, out var value) &&
            value is Queue<IDomainEvent> existingDomainEvents
            ? existingDomainEvents
            : new();

        domainEvents.ForEach(domainEventsQueue.Enqueue);
        _httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = domainEventsQueue;
    }
}