namespace ex03_ef_postgresql.Exceptions;

public class PassportAlreadyExistsException : ArgumentException
{
    public PassportAlreadyExistsException(string passportNumber) 
        : base($"Traveler with passport number {passportNumber} already exists.")
    {
    }
}

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string message) : base(message)
    {
    }
}