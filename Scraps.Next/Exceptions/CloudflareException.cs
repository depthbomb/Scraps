namespace Scraps.Next.Exceptions;

public class CloudflareException : Exception
{
    public CloudflareException() { }
    public CloudflareException(string message) : base(message) { }
    public CloudflareException(string message, Exception inner) : base(message, inner) { }
}