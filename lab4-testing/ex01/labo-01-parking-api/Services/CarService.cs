namespace labo_01_parking_api.Services;

public interface ICarService
{
    Task<CarDto?> CreateCarAsync(CreateCarDto dto, CancellationToken ct = default);
    Task<List<CarDto>> GetAllCarsAsync(CancellationToken ct = default);
    Task<CarDto?> GetCarByIdAsync(int id, CancellationToken ct = default);
}

public class CarService(ICarRepository repository, IMapper mapper) : ICarService
{
    public async Task<CarDto?> CreateCarAsync(CreateCarDto dto, CancellationToken ct = default)
    {
        var car = mapper.Map<Car>(dto);
        await repository.AddAsync(car, ct);
        return mapper.Map<CarDto>(car);
    }

    public async Task<List<CarDto>> GetAllCarsAsync(CancellationToken ct = default)
    {
        var cars = await repository.GetAllAsync(ct);
        return mapper.Map<List<CarDto>>(cars);
    }

    public async Task<CarDto?> GetCarByIdAsync(int id, CancellationToken ct = default)
    {
        var car = await repository.GetByIdAsync(id, ct);
        return mapper.Map<CarDto>(car);
    }
}
