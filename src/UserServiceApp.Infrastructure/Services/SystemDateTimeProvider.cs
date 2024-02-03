using UserServiceApp.Contracts.Common.Interfaces;

namespace UserServiceApp.Infrastructure.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
