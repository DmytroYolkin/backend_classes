namespace ex07.Exceptions;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string resourceName, object resourceId)
        : base($"{resourceName} with ID {resourceId} was not found.")
    {
    }
}
