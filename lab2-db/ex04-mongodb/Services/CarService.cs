namespace Howest.Lab2.Ex04.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<Car> _carValidator;
    private readonly IValidator<Brand> _brandValidator;

    public CarService(
        ICarRepository carRepository, 
        IBrandRepository brandRepository, 
        IMapper mapper, 
        IValidator<Car> carValidator, 
        IValidator<Brand> brandValidator)
    {
        _carRepository = carRepository;
        _brandRepository = brandRepository;
        _mapper = mapper;
        _carValidator = carValidator;
        _brandValidator = brandValidator;
    }

    public async Task SetupDummyData()
    {
        var existingBrands = await _brandRepository.GetAllBrands();
        if (!existingBrands.Any())
        {
            var brands = new List<Brand>(){
                new Brand() { Country = "Germany", Name = "Volkswagen" },
                new Brand() { Country = "Germany", Name = "BMW" },
                new Brand() { Country = "Germany", Name = "Audi" },
                new Brand() { Country = "USA", Name = "Tesla" }
            };

            foreach (var brand in brands)
                await _brandRepository.AddBrand(brand);
        }

        var existingCars = await _carRepository.GetAllCars();
        if (!existingCars.Any())
        {
            var brands = (await _brandRepository.GetAllBrands()).ToList();
            var cars = new List<Car>()
            {
                new Car() { Name = "ID.3", Brand = brands[0] },
                new Car() { Name = "ID.4", Brand = brands[0] },
                new Car() { Name = "IX3", Brand = brands[1] },
                new Car() { Name = "E-Tron", Brand = brands[2] },
                new Car() { Name = "Model Y", Brand = brands[3] }
            };
            foreach (var car in cars)
                await _carRepository.AddCar(car);
        }
    }

    public async Task<IEnumerable<CarDTO>> GetCars()
    {
        var cars = await _carRepository.GetAllCars();
        return _mapper.Map<IEnumerable<CarDTO>>(cars);
    }

    public async Task<IEnumerable<BrandDTO>> GetBrands()
    {
        var brands = await _brandRepository.GetAllBrands();
        return _mapper.Map<IEnumerable<BrandDTO>>(brands);
    }

    public async Task<CarDTO> AddCar(CarDTO carDto)
    {
        var car = _mapper.Map<Car>(carDto);

        var validationResult = await _carValidator.ValidateAsync(car);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _carRepository.AddCar(car);
        return _mapper.Map<CarDTO>(car);
    }

    public async Task<BrandDTO> AddBrand(BrandDTO brandDto)
    {
        var brand = _mapper.Map<Brand>(brandDto);

        var validationResult = await _brandValidator.ValidateAsync(brand);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _brandRepository.AddBrand(brand);
        return _mapper.Map<BrandDTO>(brand);
    }
}