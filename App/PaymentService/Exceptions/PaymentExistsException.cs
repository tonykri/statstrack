using System;

public class PaymentExistsException : Exception
{
    public PaymentExistsException()
    {
    }

    public PaymentExistsException(string message) : base(message)
    {
    }

    public PaymentExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}