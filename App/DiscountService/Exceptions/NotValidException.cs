using System;

public class NotValidException : Exception
{
    public NotValidException()
    {
    }

    public NotValidException(string message) : base(message)
    {
    }

    public NotValidException(string message, Exception innerException) : base(message, innerException)
    {
    }
}