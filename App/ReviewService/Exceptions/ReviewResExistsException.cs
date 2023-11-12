using System;

public class ReviewResExistsException : Exception
{
    public ReviewResExistsException()
    {
    }

    public ReviewResExistsException(string message) : base(message)
    {
    }

    public ReviewResExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}