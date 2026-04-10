using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Howest.lab2.ex04_sensor_monitoring.Models;

namespace Howest.lab2.ex04_sensor_monitoring.Data;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string SensorCollectionName { get; set; } = "Sensors";
    public string ReadingCollectionName { get; set; } = "Readings";
}

public interface ISensorRepository
{
    Task<List<Sensor>> GetAllAsync();
    Task<Sensor?> GetByIdAsync(string id);
    Task CreateAsync(Sensor sensor);
    Task DeleteAsync(string id);
}

public interface IReadingRepository
{
    Task CreateAsync(Reading reading);
    Task<List<Reading>> GetBySensorIdAsync(string sensorId);
}

public class SensorRepository : ISensorRepository
{
    private readonly IMongoCollection<Sensor> _sensors;

    public SensorRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _sensors = database.GetCollection<Sensor>(settings.Value.SensorCollectionName);
    }

    public async Task<List<Sensor>> GetAllAsync() => await _sensors.Find(_ => true).ToListAsync();
    public async Task<Sensor?> GetByIdAsync(string id) => await _sensors.Find(x => x.Id == id).FirstOrDefaultAsync();
    public async Task CreateAsync(Sensor sensor) => await _sensors.InsertOneAsync(sensor);
    public async Task DeleteAsync(string id) => await _sensors.DeleteOneAsync(x => x.Id == id);
}

public class ReadingRepository : IReadingRepository
{
    private readonly IMongoCollection<Reading> _readings;

    public ReadingRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _readings = database.GetCollection<Reading>(settings.Value.ReadingCollectionName);
    }

    public async Task CreateAsync(Reading reading) => await _readings.InsertOneAsync(reading);
    public async Task<List<Reading>> GetBySensorIdAsync(string sensorId) => await _readings.Find(x => x.SensorId == sensorId).ToListAsync();
}
