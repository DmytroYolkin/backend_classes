namespace Howest.Lab2.Ex04.Validations;

public class CarValidator : AbstractValidator<Car>
{
    public CarValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is mandatory for a Car.");
        RuleFor(x => x.Brand).NotNull().WithMessage("Brand is mandatory for a Car.");
    }
}