namespace Howest.Lab1.Cars.Validations;

public class CarValidator: AbstractValidator<CarModel>
{
    public CarValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(c => c.Brand).NotNull().WithMessage("Brand is required");
        When(c => c.Brand is not null, () =>
        {
            RuleFor(c => c.Brand).SetValidator(new BrandValidator());
        });
    }
}

public class BrandValidator: AbstractValidator<Brand>
{
    public BrandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).WithMessage("Name is required and must be at least 2 characters long");
        RuleFor(b => b.Country).NotEmpty().MinimumLength(2).WithMessage("Country is required and must be at least 2 characters long");
        RuleFor(b => b.Logo).NotEmpty().MinimumLength(2).WithMessage("Logo is required");
    }
}