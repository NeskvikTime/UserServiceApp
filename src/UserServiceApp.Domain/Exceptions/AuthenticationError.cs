namespace UserServiceApp.Domain.Exceptions;
public class AuthenticationError : Exception
{
    public AuthenticationError(string message) : base(message)
    {

    }
}
