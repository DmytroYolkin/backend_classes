namespace labo_01_parking_test.Repositories;

internal class FakeCarRepository : ICarRepository
{
    private readonly List<Car> _cars = [];

    public Task<Car?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_cars.FirstOrDefault(c => c.Id == id));

    public Task<Car?> GetByPlateAsync(string plate, CancellationToken ct = default)
        => Task.FromResult(_cars.FirstOrDefault(c => c.Plate == plate));

    public Task<List<Car>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult(new List<Car>(_cars));

    public Task AddAsync(Car car, CancellationToken ct = default)
    {
        if (_cars.Count == 0)
            car.Id = 1;
        else
            car.Id = _cars.Max(c => c.Id) + 1;
        
        _cars.Add(car);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Car car, CancellationToken ct = default)
    {
        var existing = _cars.FirstOrDefault(c => c.Id == car.Id);
        if (existing != null)
        {
            _cars.Remove(existing);
            _cars.Add(car);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var car = _cars.FirstOrDefault(c => c.Id == id);
        if (car != null)
            _cars.Remove(car);
        return Task.CompletedTask;
    }
}

internal class FakeRegistrationRepository : IRegistrationRepository
{
    private readonly List<Registration> _registrations = [];

    public Task<Registration?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_registrations.FirstOrDefault(r => r.Id == id));

    public Task<List<Registration>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult(new List<Registration>(_registrations));

    public Task<Registration?> GetActiveByPlateAsync(string plate, CancellationToken ct = default)
        => Task.FromResult(_registrations.FirstOrDefault(r => r.Plate == plate && !r.IsFinished));

    public Task AddAsync(Registration registration, CancellationToken ct = default)
    {
        if (_registrations.Count == 0)
            registration.Id = 1;
        else
            registration.Id = _registrations.Max(r => r.Id) + 1;
        
        _registrations.Add(registration);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Registration registration, CancellationToken ct = default)
    {
        var existing = _registrations.FirstOrDefault(r => r.Id == registration.Id);
        if (existing != null)
        {
            _registrations.Remove(existing);
            _registrations.Add(registration);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var registration = _registrations.FirstOrDefault(r => r.Id == id);
        if (registration != null)
            _registrations.Remove(registration);
        return Task.CompletedTask;
    }
}
