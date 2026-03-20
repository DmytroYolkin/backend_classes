namespace Howest.Lab1.Ex3.Services;
public interface IVaccinationService
{
    VaccinRegistration AddRegistration(VaccinRegistration registration);
    List<VaccinationLocation> GetLocations();
    List<VaccinRegistration> GetRegistrations();
    List<VaccinType> GetVaccins();
    VaccinType GetVaccinById(Guid id);
    VaccinationLocation GetLocationById(Guid id);
}

public class VaccinationService : IVaccinationService
{
    private readonly IVaccinationRegistrationRepository _vaccinationRegistrationRepository;
    private readonly IVaccinationLocationRepository _vaccinationLocationRepository;
    private readonly IVaccinTypeRepository _vaccinTypeRepository;

    public VaccinationService(IVaccinationRegistrationRepository vaccinationRegistrationRepository, IVaccinationLocationRepository vaccinationLocationRepository, IVaccinTypeRepository vaccinTypeRepository)
    {
        _vaccinationRegistrationRepository = vaccinationRegistrationRepository;
        _vaccinationLocationRepository = vaccinationLocationRepository;
        _vaccinTypeRepository = vaccinTypeRepository;
    }

    public VaccinRegistration AddRegistration(VaccinRegistration registration)
    {
        return _vaccinationRegistrationRepository.AddRegistration(registration);
    }

    public List<VaccinationLocation> GetLocations()
    {
        return _vaccinationLocationRepository.GetLocations();
    }

    public List<VaccinRegistration> GetRegistrations()
    {
        return _vaccinationRegistrationRepository.GetRegistrations();
    }

    public List<VaccinType> GetVaccins()
    {
        return _vaccinTypeRepository.GetVaccinTypes();
    }

    public VaccinType GetVaccinById(Guid id)
    {
        return _vaccinTypeRepository.GetVaccinTypes().FirstOrDefault(v => v.VaccinTypeId == id);
    }

    public VaccinationLocation GetLocationById(Guid id)
    {
        return _vaccinationLocationRepository.GetLocations().FirstOrDefault(l => l.VaccinationLocationId == id);
    }
}