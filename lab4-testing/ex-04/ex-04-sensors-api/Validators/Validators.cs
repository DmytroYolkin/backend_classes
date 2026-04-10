using FluentValidation;
using Howest.lab2.ex04_sensor_monitoring.DTOs;

namespace Howest.lab2.ex04_sensor_monitoring.Validators;

public class SensorCreateValidator : AbstractValidator<SensorCreateDto>
{
    public SensorCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Location).MaximumLength(200);
        RuleFor(x => x.PhoneNumber).Matches(@"^\+?[1-9]\d{1,14}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Invalid phone number format.");
    }
}

public class ReadingCreateValidator : AbstractValidator<ReadingCreateDto>
{
    public ReadingCreateValidator()
    {
        RuleFor(x => x.Value).NotNull();
    }
}
