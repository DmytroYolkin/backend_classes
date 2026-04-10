using Xunit;
using Howest.lab2.ex04_sensor_monitoring.Validators;
using Howest.lab2.ex04_sensor_monitoring.DTOs;

namespace Howest.lab2.ex04_sensor_monitoring.Tests;

public class ValidatorTests
{
    private readonly SensorCreateValidator _sensorValidator;
    private readonly ReadingCreateValidator _readingValidator;

    public ValidatorTests()
    {
        _sensorValidator = new SensorCreateValidator();
        _readingValidator = new ReadingCreateValidator();
    }

    [Fact]
    public void SensorValidator_ShouldHaveError_WhenNameIsEmpty()
    {
        var dto = new SensorCreateDto("", "Living Room", "+32470001111");
        var result = _sensorValidator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void SensorValidator_ShouldHaveError_WhenPhoneNumberIsInvalid()
    {
        var dto = new SensorCreateDto("Temp Sensor", "Lab", "0470-11-22-33"); // invalid + format
        var result = _sensorValidator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PhoneNumber");
    }

    [Fact]
    public void SensorValidator_ShouldBeValid_WhenDataIsCorrect()
    {
        var dto = new SensorCreateDto("Valid Sensor", "Kitchen", "+32470001111");
        var result = _sensorValidator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
