using FluentValidation;
using Howest.Lab1.Models;   

namespace Howest.Lab1.Validations;

public class WineValidator: FluentValidation.AbstractValidator<Wine>
{
    public WineValidator()
    {
        RuleFor(w => w.Name)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Wine name is required.");
        RuleFor(w => w.Year)
            .InclusiveBetween(1900, DateTime.Now.Year)
            .WithMessage("Wine year must be between 1900 and the current year.");
        RuleFor(w => w.Country)
            .NotEmpty()
            .WithMessage("Wine country is required.");
        RuleFor(w => w.Color)
            .NotEmpty()
            .WithMessage("Wine color is required.");
        RuleFor(w => w.Price)
            .GreaterThan(0)
            .WithMessage("Wine price must be a positive number.");
        RuleFor(w => w.Grapes)
            .NotEmpty()
            .WithMessage("Wine grapes are required.");
    }
}