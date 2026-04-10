namespace labo_01_parking_api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCarDto, Car>();
        CreateMap<Car, CarDto>();

        CreateMap<CreateRegistrationDto, Registration>();
        CreateMap<Registration, RegistrationDto>();
        CreateMap<Registration, RegistrationResponseDto>();
    }
}
