namespace AudiSoft.Domain.Exceptions;

public class ValidationAppException : Exception
{
    public ValidationAppException(string message) : base(message) { }
}
