using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UserServiceApp.Domain.Common;
using UserServiceApp.Infrastructure.Persistance;

namespace UserServiceApp.Infrastructure.Common;
public class EventualConsistencyMiddleware(RequestDelegate _next, ILogger<EventualConsistencyMiddleware> _logger)
{
    public const string DomainEventsKey = "DomainEventsKey";

    internal async Task InvokeAsync(HttpContext context, IPublisher publisher, ApplicationDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();

        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue("DomainEventsQueue", out var value) &&
                    value is Queue<IDomainEvent> domainEventsQueue)
                {
                    while (domainEventsQueue!.TryDequeue(out var domainEvent))
                    {
                        await publisher.Publish(domainEvent);
                    }
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                _logger.LogWarning("Eventual consistency exception occurred!");
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }
}
