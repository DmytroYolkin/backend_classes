namespace Howest.Lab1.Ex3.DTO;

public class VaccinRegistrationDTO
{
    public Guid VaccinatinRegistrationId { get; set; }
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? EMail { get; set; }
    public int YearOfBirth { get; set; }
    public string? VaccinationDate { get; set; }
    public string? VaccinName { get; set; }
    public string? Location { get; set; }
}

public class DTOProfile : Profile{
    public DTOProfile()
    {
        CreateMap<VaccinRegistration, VaccinRegistrationDTO>()
        .ForMember(dest => dest.VaccinName , opt => opt.MapFrom<VaccinResolver>())
        .ForMember(dest => dest.Location , opt => opt.MapFrom<VaccinLocationResolver>());
    }
}

public class VaccinLocationResolver : IValueResolver<VaccinRegistration, VaccinRegistrationDTO,string>{
    public string Resolve(VaccinRegistration source, VaccinRegistrationDTO destination,string dest, ResolutionContext context)
    {
        if (context.Items.TryGetValue("locations", out var locObj) && locObj is List<VaccinationLocation> locations)
        {
            var location = locations.FirstOrDefault(l => l.VaccinationLocationId == source.VaccinationLocationId);
            return location?.Name ?? "Unknown Location";
        }
        return "Unknown Location";
    }
}

public class VaccinResolver : IValueResolver<VaccinRegistration, VaccinRegistrationDTO,string>{
    public string Resolve(VaccinRegistration source, VaccinRegistrationDTO destination,string dest, ResolutionContext context)
    {
        if (context.Items.TryGetValue("vaccins", out var vacObj) && vacObj is List<VaccinType> vaccins)
        {
            var vaccin = vaccins.FirstOrDefault(l => l.VaccinTypeId == source.VaccinTypeId);
            return vaccin?.Name ?? "Unknown Vaccine";
        }
        return "Unknown Vaccine";
    }
}