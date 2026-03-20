namespace Howest.Lab1.Ex3.Repositories;

public interface IVaccinTypeRepository
{
    VaccinType AddVaccinType(VaccinType vaccinType);
    List<VaccinType> GetVaccinTypes();
}

public class VaccinTypeRepository : IVaccinTypeRepository
{
    private static List<VaccinType> _vaccinTypes = new List<VaccinType>()
    {
        new VaccinType()
        {
            VaccinTypeId = Guid.Parse("d1c8e5b2-9c3f-4a1e-8b6f-2a7e5d9f0c3a"),
            Name = "Pfizer"
        },
        new VaccinType()
        {
            VaccinTypeId = Guid.Parse("a2b7c9d4-5e6f-4b8a-9c1d-3f4e5a6b7c8d"),
            Name = "Moderna"
        },
        new VaccinType()
        {
            VaccinTypeId = Guid.Parse("e3f9a1b6-7c8d-4e9f-0a1b-2c3d4e5f6a7b"),
            Name = "AstraZeneca"
        }
    };

    public VaccinType AddVaccinType(VaccinType vaccinType)
    {
        vaccinType.VaccinTypeId = Guid.NewGuid();
        _vaccinTypes.Add(vaccinType);
        return vaccinType;
    }

    public List<VaccinType> GetVaccinTypes()
    {
        return _vaccinTypes;
    }
}