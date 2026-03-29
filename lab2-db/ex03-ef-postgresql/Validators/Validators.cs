using FluentValidation;
using ex03_ef_postgresql.Models;

namespace ex03_ef_postgresql.Validators;

public class TravelerValidator : AbstractValidator<Traveler>
{
    public TravelerValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

        RuleFor(x => x.Passport)
            .NotNull().WithMessage("Passport is required");

        RuleFor(x => x.Passport!.PassportNumber)
            .NotEmpty().WithMessage("Passport number is required")
            .MaximumLength(10).WithMessage("Passport number cannot exceed 10 characters");
    }
}

public class DestinationValidator : AbstractValidator<Destination>
{
    public DestinationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");
    }
}
