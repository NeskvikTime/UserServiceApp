namespace UserServiceApp.Domain.Exceptions;
public class UserServiceAppException : Exception
{
    public UserServiceAppException(string message) : base(message)
    {
    }
}
