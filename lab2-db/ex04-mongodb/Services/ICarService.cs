namespace Howest.Lab2.Ex04.Services;

public interface ICarService
{
    Task SetupDummyData();
    Task<IEnumerable<CarDTO>> GetCars();
    Task<IEnumerable<BrandDTO>> GetBrands();
    Task<CarDTO> AddCar(CarDTO carDto);
    Task<BrandDTO> AddBrand(BrandDTO brandDto);
}