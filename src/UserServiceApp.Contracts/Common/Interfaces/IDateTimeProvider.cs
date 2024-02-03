namespace UserServiceApp.Contracts.Common.Interfaces;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}