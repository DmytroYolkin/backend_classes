namespace Howest.lab2.ex04_sensor_monitoring.DTOs;

public record SensorCreateDto(string Name, string? Location, string? PhoneNumber);
public record SensorResponseDto(string Id, string Name, string? Location, string? PhoneNumber);

public record ReadingCreateDto(decimal Value);
public record ReadingResponseDto(string Id, string SensorId, decimal Value, DateTime Timestamp);
