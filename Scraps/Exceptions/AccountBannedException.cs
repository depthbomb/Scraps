namespace Scraps.Exceptions;

public class AccountBannedException : Exception
{
    public AccountBannedException() {}
    public AccountBannedException(string message) : base(message) {}
    public AccountBannedException(string message, Exception inner) : base(message, inner) {}
}