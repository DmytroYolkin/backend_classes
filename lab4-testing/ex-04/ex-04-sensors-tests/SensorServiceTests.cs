using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Xunit;
using Howest.lab2.ex04_sensor_monitoring.Services;
using Howest.lab2.ex04_sensor_monitoring.Data;
using Howest.lab2.ex04_sensor_monitoring.Models;
using Howest.lab2.ex04_sensor_monitoring.DTOs;

namespace Howest.lab2.ex04_sensor_monitoring.Tests;

public class SensorServiceTests
{
    private readonly Mock<ISensorRepository> _sensorRepoMock;
    private readonly Mock<IReadingRepository> _readingRepoMock;
    private readonly Mock<ISmsService> _smsServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<SensorService>> _loggerMock;
    private readonly SensorService _service;

    public SensorServiceTests()
    {
        _sensorRepoMock = new Mock<ISensorRepository>();
        _readingRepoMock = new Mock<IReadingRepository>();
        _smsServiceMock = new Mock<ISmsService>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<SensorService>>();

        _service = new SensorService(
            _sensorRepoMock.Object,
            _readingRepoMock.Object,
            _smsServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task AddReadingAsync_ShouldSendSms_WhenValueIsBetween20And50AndPhoneExists()
    {
        // Arrange
        var sensorId = "sensor123";
        var sensor = new Sensor { Id = sensorId, Name = "Test Sensor", PhoneNumber = "+32470000000" };
        var dto = new ReadingCreateDto(35); // Within 20-50

        _sensorRepoMock.Setup(r => r.GetByIdAsync(sensorId)).ReturnsAsync(sensor);
        
        // Act
        await _service.AddReadingAsync(sensorId, dto);

        // Assert
        _smsServiceMock.Verify(s => s.SendSmsAsync(sensor.PhoneNumber, It.Is<string>(m => m.Contains("35"))), Times.Once);
        _readingRepoMock.Verify(r => r.CreateAsync(It.Is<Reading>(rd => rd.Value == 35 && rd.SensorId == sensorId)), Times.Once);
    }

    [Fact]
    public async Task AddReadingAsync_ShouldNotSendSms_WhenValueIsOutsideRange()
    {
        // Arrange
        var sensorId = "sensor123";
        var sensor = new Sensor { Id = sensorId, Name = "Test Sensor", PhoneNumber = "+32470000000" };
        var dto = new ReadingCreateDto(15); // Outside 20-50

        _sensorRepoMock.Setup(r => r.GetByIdAsync(sensorId)).ReturnsAsync(sensor);

        // Act
        await _service.AddReadingAsync(sensorId, dto);

        // Assert
        _smsServiceMock.Verify(s => s.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _readingRepoMock.Verify(r => r.CreateAsync(It.IsAny<Reading>()), Times.Once);
    }

    [Fact]
    public async Task AddReadingAsync_ShouldNotSendSms_WhenPhoneNumberIsMissing()
    {
        // Arrange
        var sensorId = "sensor123";
        var sensor = new Sensor { Id = sensorId, Name = "Test Sensor", PhoneNumber = null };
        var dto = new ReadingCreateDto(35); // Inside range but no phone

        _sensorRepoMock.Setup(r => r.GetByIdAsync(sensorId)).ReturnsAsync(sensor);

        // Act
        await _service.AddReadingAsync(sensorId, dto);

        // Assert
        _smsServiceMock.Verify(s => s.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetSensorAsync_ShouldReturnNull_WhenSensorDoesNotExist()
    {
        // Arrange
        _sensorRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Sensor?)null);

        // Act
        var result = await _service.GetSensorAsync("invalid");

        // Assert
        Assert.Null(result);
    }
}
