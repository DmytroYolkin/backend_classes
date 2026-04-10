namespace labo_01_parking_api.Validators;

public interface ILicensePlateValidator
{
    bool IsValidLicensePlate(string plate);
}

public class LicensePlateValidator : ILicensePlateValidator
{
    public bool IsValidLicensePlate(string plate)
    {
        if (string.IsNullOrWhiteSpace(plate))
            return false;

        plate = plate.Trim().ToUpperInvariant();
        
        return System.Text.RegularExpressions.Regex.IsMatch(plate, @"^[A-Z0-9\-]{3,20}$");
    }
}

public class CreateCarValidator : AbstractValidator<CreateCarDto>
{
    private readonly ILicensePlateValidator _licensePlateValidator;

    public CreateCarValidator(ILicensePlateValidator licensePlateValidator)
    {
        _licensePlateValidator = licensePlateValidator;

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required")
            .MaximumLength(50).WithMessage("Brand must not exceed 50 characters");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required")
            .MaximumLength(50).WithMessage("Model must not exceed 50 characters");

        RuleFor(x => x.Plate)
            .NotEmpty().WithMessage("License plate is required")
            .Must(plate => _licensePlateValidator.IsValidLicensePlate(plate))
            .WithMessage("License plate format is invalid");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Color is required")
            .MaximumLength(30).WithMessage("Color must not exceed 30 characters");
    }
}

public class CreateRegistrationValidator : AbstractValidator<CreateRegistrationDto>
{
    private readonly ILicensePlateValidator _licensePlateValidator;

    public CreateRegistrationValidator(ILicensePlateValidator licensePlateValidator)
    {
        _licensePlateValidator = licensePlateValidator;

        RuleFor(x => x.Plate)
            .NotEmpty().WithMessage("License plate is required")
            .Must(plate => _licensePlateValidator.IsValidLicensePlate(plate))
            .WithMessage("License plate format is invalid");

        RuleFor(x => x.CarId)
            .GreaterThan(0).WithMessage("Car ID must be valid");
    }
}

public class UpdateRegistrationValidator : AbstractValidator<UpdateRegistrationDto>
{
    public UpdateRegistrationValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Registration ID must be valid");

        RuleFor(x => x.CarId)
            .GreaterThan(0).WithMessage("Car ID must be valid");
    }
}
