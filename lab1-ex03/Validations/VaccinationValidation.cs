namespace Howest.Lab1.Ex3.Validations;

public class VaccinationRegistrationValidator : AbstractValidator<VaccinRegistration>
{
    public VaccinationRegistrationValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
        RuleFor(x => x.YearOfBirth).NotEmpty().WithMessage("Year of birth is required.");
        RuleFor(x => x.VaccinTypeId).NotEmpty().WithMessage("Vaccin type is required.");
        RuleFor(x => x.VaccinationDate).NotEmpty().WithMessage("Vaccination date is required.");
        RuleFor(x => x.VaccinationLocationId).NotEmpty().WithMessage("Vaccination location is required.");
        RuleFor(x => x.YearOfBirth).InclusiveBetween(1900, DateTime.Now.Year).WithMessage("Year of birth must be between 1900 and the current year.");
    }
}