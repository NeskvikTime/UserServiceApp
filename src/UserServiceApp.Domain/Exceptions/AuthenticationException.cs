﻿namespace UserServiceApp.Domain.Exceptions;
public class AuthenticationException : Exception
{
    public AuthenticationException(string message) : base(message)
    {

    }

    public AuthenticationException() : base()
    {
    }

    public AuthenticationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
