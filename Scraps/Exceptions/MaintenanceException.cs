namespace Scraps.Exceptions;

public class MaintenanceException : Exception
{
    public MaintenanceException() { }
    public MaintenanceException(string message) : base(message) { }
    public MaintenanceException(string message, Exception inner) : base(message, inner) { }
}