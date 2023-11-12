using System;

public class ProfilePhotoException : Exception
{
    public ProfilePhotoException()
    {
    }

    public ProfilePhotoException(string message) : base(message)
    {
    }

    public ProfilePhotoException(string message, Exception innerException) : base(message, innerException)
    {
    }
}