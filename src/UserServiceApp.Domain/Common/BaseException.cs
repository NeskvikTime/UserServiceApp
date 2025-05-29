namespace UserServiceApp.Domain.Common;

public abstract class BaseException : Exception
{
    public BaseException(string message) : base(message)
    {

    }

    public BaseException() : base()
    {
    }

    public BaseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
