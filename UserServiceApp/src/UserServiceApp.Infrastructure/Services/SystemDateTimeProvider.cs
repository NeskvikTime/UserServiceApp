using UserServiceApp.Domain.Common.Interfaces;

namespace UserServiceApp.Infrastructure.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
