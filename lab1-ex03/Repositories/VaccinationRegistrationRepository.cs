namespace Howest.Lab1.Ex3.Repositories;

public interface IVaccinationRegistrationRepository
{
    VaccinRegistration AddRegistration(VaccinRegistration registration);
    List<VaccinRegistration> GetRegistrations();
    VaccinRegistration GetOneById(Guid id);
}

public class VaccinationRegistrationRepository : IVaccinationRegistrationRepository
{
    private static List<VaccinRegistration> _registrations = new List<VaccinRegistration>();

    public VaccinRegistration AddRegistration(VaccinRegistration registration)
    {
        registration.VaccinatinRegistrationId = Guid.NewGuid();
        _registrations.Add(registration);
        return registration;
    }

    public List<VaccinRegistration> GetRegistrations()
    {
        return _registrations;
    }

    public VaccinRegistration GetOneById(Guid id)
    {
        return _registrations.FirstOrDefault(r => r.VaccinatinRegistrationId == id);
    }

}