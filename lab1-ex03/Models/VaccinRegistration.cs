namespace Howest.Lab1.Ex3.Models;

public class VaccinRegistration
{
    public Guid VaccinatinRegistrationId { get; set; }
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? Email { get; set; }
    public int YearOfBirth { get; set; }
    public string? VaccinationDate { get; set; }
    public Guid VaccinTypeId { get; set; }
    public Guid VaccinationLocationId { get; set; }
}
