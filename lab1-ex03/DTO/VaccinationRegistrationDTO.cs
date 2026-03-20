using AutoMapper;
using Howest.Lab1.Ex3.Models;
using Howest.Lab1.Ex3.Services;

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

public class VaccinLocationResolver : IValueResolver<VaccinRegistration, VaccinRegistrationDTO, string>
{
    private readonly IVaccinationService _vaccinationService;

    public VaccinLocationResolver(IVaccinationService vaccinationService)
    {
        _vaccinationService = vaccinationService;
    }

    public string Resolve(VaccinRegistration source, VaccinRegistrationDTO destination, string dest, ResolutionContext context)
    {
        return _vaccinationService.GetLocationById(source.VaccinationLocationId)?.Name ?? "Unknown Location";
    }
}

public class VaccinResolver : IValueResolver<VaccinRegistration, VaccinRegistrationDTO, string>
{
    private readonly IVaccinationService _vaccinationService;

    public VaccinResolver(IVaccinationService vaccinationService)
    {
        _vaccinationService = vaccinationService;
    }

    public string Resolve(VaccinRegistration source, VaccinRegistrationDTO destination, string dest, ResolutionContext context)
    {
        return _vaccinationService.GetVaccinById(source.VaccinTypeId)?.Name ?? "Unknown Vaccine";
    }
}