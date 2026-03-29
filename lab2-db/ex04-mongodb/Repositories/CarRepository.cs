namespace Howest.Lab2.Ex04.Repositories;

public class CarRepository : ICarRepository
{
    private readonly IMongoContext _context;

    public CarRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task AddCar(Car car)
    {
        await _context.CarsCollection.InsertOneAsync(car);
    }

    public async Task<IEnumerable<Car>> GetAllCars()
    {
        return await _context.CarsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Car> GetCar(string id)
    {
        return await _context.CarsCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
    }
}