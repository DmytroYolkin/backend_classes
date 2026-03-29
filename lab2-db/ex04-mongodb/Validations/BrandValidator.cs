namespace Howest.Lab2.Ex04.Validations;

public class BrandValidator : AbstractValidator<Brand>
{
    public BrandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is mandatory for a Brand.");
        RuleFor(x => x.Country).NotEmpty().WithMessage("Country is mandatory for a Brand.");
    }
}