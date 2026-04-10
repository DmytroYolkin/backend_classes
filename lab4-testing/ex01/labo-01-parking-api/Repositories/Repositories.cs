namespace labo_01_parking_api.Repositories;

public interface ICarRepository
{
    Task<Car?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Car?> GetByPlateAsync(string plate, CancellationToken ct = default);
    Task<List<Car>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Car car, CancellationToken ct = default);
    Task UpdateAsync(Car car, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

public class CarRepository(ParkingDbContext context) : ICarRepository
{
    public Task<Car?> GetByIdAsync(int id, CancellationToken ct = default)
        => context.Cars.FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<Car?> GetByPlateAsync(string plate, CancellationToken ct = default)
        => context.Cars.FirstOrDefaultAsync(c => c.Plate == plate, ct);

    public Task<List<Car>> GetAllAsync(CancellationToken ct = default)
        => context.Cars.ToListAsync(ct);

    public async Task AddAsync(Car car, CancellationToken ct = default)
    {
        await context.Cars.AddAsync(car, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Car car, CancellationToken ct = default)
    {
        context.Cars.Update(car);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var car = await GetByIdAsync(id, ct);
        if (car != null)
        {
            context.Cars.Remove(car);
            await context.SaveChangesAsync(ct);
        }
    }
}

public interface IRegistrationRepository
{
    Task<Registration?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Registration>> GetAllAsync(CancellationToken ct = default);
    Task<Registration?> GetActiveByPlateAsync(string plate, CancellationToken ct = default);
    Task AddAsync(Registration registration, CancellationToken ct = default);
    Task UpdateAsync(Registration registration, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

public class RegistrationRepository(ParkingDbContext context) : IRegistrationRepository
{
    public Task<Registration?> GetByIdAsync(int id, CancellationToken ct = default)
        => context.Registrations.FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<List<Registration>> GetAllAsync(CancellationToken ct = default)
        => context.Registrations.ToListAsync(ct);

    public Task<Registration?> GetActiveByPlateAsync(string plate, CancellationToken ct = default)
        => context.Registrations.FirstOrDefaultAsync(r => r.Plate == plate && !r.IsFinished, ct);

    public async Task AddAsync(Registration registration, CancellationToken ct = default)
    {
        await context.Registrations.AddAsync(registration, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Registration registration, CancellationToken ct = default)
    {
        context.Registrations.Update(registration);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var registration = await GetByIdAsync(id, ct);
        if (registration != null)
        {
            context.Registrations.Remove(registration);
            await context.SaveChangesAsync(ct);
        }
    }
}
