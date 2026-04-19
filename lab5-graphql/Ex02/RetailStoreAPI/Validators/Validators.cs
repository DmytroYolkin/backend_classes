using FluentValidation;
using RetailStoreAPI.DTOs;

namespace RetailStoreAPI.Validators;

public class AddProductValidator : AbstractValidator<AddProductInput>
{
    public AddProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CategoryId).GreaterThan(0);
    }
}

public class AddCategoryValidator : AbstractValidator<AddCategoryInput>
{
    public AddCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}

public class AddDiscountValidator : AbstractValidator<AddDiscountInput>
{
    public AddDiscountValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Percentage).InclusiveBetween(1, 100);
        RuleFor(x => x.ExpiresAt).GreaterThan(DateTime.UtcNow).When(x => x.ExpiresAt.HasValue);
    }
}

public class AddEmployeeValidator : AbstractValidator<AddEmployeeInput>
{
    public AddEmployeeValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Role).NotEmpty();
        RuleFor(x => x.HiredAt).LessThanOrEqualTo(DateTime.UtcNow);
    }
}
