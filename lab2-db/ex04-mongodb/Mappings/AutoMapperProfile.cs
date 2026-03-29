namespace Howest.Lab2.Ex04.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Brand, BrandDTO>().ReverseMap();
        CreateMap<Car, CarDTO>().ReverseMap();
    }
}