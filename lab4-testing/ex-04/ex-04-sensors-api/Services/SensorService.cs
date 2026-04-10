using AutoMapper;
using Howest.lab2.ex04_sensor_monitoring.DTOs;
using Howest.lab2.ex04_sensor_monitoring.Models;
using Howest.lab2.ex04_sensor_monitoring.Data;

namespace Howest.lab2.ex04_sensor_monitoring.Services;

public interface ISensorService
{
    Task<List<SensorResponseDto>> GetSensorsAsync();
    Task<SensorResponseDto?> GetSensorAsync(string id);
    Task<SensorResponseDto> CreateSensorAsync(SensorCreateDto dto);
    Task<bool> DeleteSensorAsync(string id);
    Task<ReadingResponseDto?> AddReadingAsync(string sensorId, ReadingCreateDto dto);
    Task<List<ReadingResponseDto>> GetReadingsAsync(string sensorId);
}

public class SensorService : ISensorService
{
    private readonly ISensorRepository _sensorRepo;
    private readonly IReadingRepository _readingRepo;
    private readonly ISmsService _smsService;
    private readonly IMapper _mapper;
    private readonly ILogger<SensorService> _logger;

    public SensorService(ISensorRepository sensorRepo, IReadingRepository readingRepo, ISmsService smsService, IMapper mapper, ILogger<SensorService> logger)
    {
        _sensorRepo = sensorRepo;
        _readingRepo = readingRepo;
        _smsService = smsService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<SensorResponseDto>> GetSensorsAsync()
    {
        var sensors = await _sensorRepo.GetAllAsync();
        return _mapper.Map<List<SensorResponseDto>>(sensors);
    }

    public async Task<SensorResponseDto?> GetSensorAsync(string id)
    {
        var sensor = await _sensorRepo.GetByIdAsync(id);
        return sensor is not null ? _mapper.Map<SensorResponseDto>(sensor) : null;
    }

    public async Task<SensorResponseDto> CreateSensorAsync(SensorCreateDto dto)
    {
        var sensor = _mapper.Map<Sensor>(dto);
        await _sensorRepo.CreateAsync(sensor);
        return _mapper.Map<SensorResponseDto>(sensor);
    }

    public async Task<bool> DeleteSensorAsync(string id)
    {
        var sensor = await _sensorRepo.GetByIdAsync(id);
        if (sensor == null) return false;
        await _sensorRepo.DeleteAsync(id);
        return true;
    }

    public async Task<ReadingResponseDto?> AddReadingAsync(string sensorId, ReadingCreateDto dto)
    {
        var sensor = await _sensorRepo.GetByIdAsync(sensorId);
        if (sensor == null) return null;

        var reading = new Reading
        {
            SensorId = sensorId,
            Value = dto.Value,
            Timestamp = DateTime.UtcNow
        };

        await _readingRepo.CreateAsync(reading);

        // Business Rule: 20 <= value <= 50 -> SMS
        if (reading.Value >= 20 && reading.Value <= 50 && !string.IsNullOrEmpty(sensor.PhoneNumber))
        {
            var message = $"Warning: Sensor {sensor.Name} registered a reading of {reading.Value}.";
            await _smsService.SendSmsAsync(sensor.PhoneNumber, message);
        }

        return _mapper.Map<ReadingResponseDto>(reading);
    }

    public async Task<List<ReadingResponseDto>> GetReadingsAsync(string sensorId)
    {
        var readings = await _readingRepo.GetBySensorIdAsync(sensorId);
        return _mapper.Map<List<ReadingResponseDto>>(readings);
    }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Sensor, SensorResponseDto>();
        CreateMap<SensorCreateDto, Sensor>();
        CreateMap<Reading, ReadingResponseDto>();
    }
}
