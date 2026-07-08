namespace KiddooPlaySchool.Domain.Exceptions;

public class DomainInvariantViolationException : InvalidOperationException
{
    public DomainInvariantViolationException(string message) : base(message)
    {
    }

    public DomainInvariantViolationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
