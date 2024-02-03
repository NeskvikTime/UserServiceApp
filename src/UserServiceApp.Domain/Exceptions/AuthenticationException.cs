namespace UserServiceApp.Domain.Exceptions;
public class AuthenticationException : Exception
{
    public AuthenticationException(string message) : base(message)
    {

    }
}
