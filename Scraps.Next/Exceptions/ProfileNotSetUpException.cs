namespace Scraps.Next.Exceptions;

public class ProfileNotSetUpException : Exception
{
    public ProfileNotSetUpException() { }
    public ProfileNotSetUpException(string message) : base(message) { }
    public ProfileNotSetUpException(string message, Exception inner) : base(message, inner) { }
}