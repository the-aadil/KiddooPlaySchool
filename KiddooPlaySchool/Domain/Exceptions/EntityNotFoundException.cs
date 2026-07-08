namespace KiddooPlaySchool.Domain.Exceptions;

public class EntityNotFoundException : KeyNotFoundException
{
    public EntityNotFoundException(string entityName, Guid id)
        : base($"{entityName} with ID {id} was not found.")
    {
    }

    public EntityNotFoundException(string message) : base(message)
    {
    }
}
