namespace UserServiceApp.Domain.Exceptions;
public class UserServiceAppException : Exception
{
    public UserServiceAppException(string message) : base(message)
    {
    }

    public UserServiceAppException() : base()
    {
    }

    public UserServiceAppException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
