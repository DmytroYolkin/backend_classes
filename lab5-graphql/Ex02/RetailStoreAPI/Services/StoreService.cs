using FluentValidation;
using RetailStoreAPI.Models;
using RetailStoreAPI.Repositories;
using RetailStoreAPI.DTOs;

namespace RetailStoreAPI.Services;

public class StoreService : IStoreService
{
    private readonly IRepository<Product> _productRepo;
    private readonly IRepository<Category> _categoryRepo;
    private readonly IRepository<Discount> _discountRepo;
    private readonly IRepository<Employee> _employeeRepo;

    private readonly IValidator<AddProductInput> _productValidator;
    private readonly IValidator<AddCategoryInput> _categoryValidator;
    private readonly IValidator<AddDiscountInput> _discountValidator;
    private readonly IValidator<AddEmployeeInput> _employeeValidator;

    public StoreService(
        IRepository<Product> productRepo,
        IRepository<Category> categoryRepo,
        IRepository<Discount> discountRepo,
        IRepository<Employee> employeeRepo,
        IValidator<AddProductInput> productValidator,
        IValidator<AddCategoryInput> categoryValidator,
        IValidator<AddDiscountInput> discountValidator,
        IValidator<AddEmployeeInput> employeeValidator)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _discountRepo = discountRepo;
        _employeeRepo = employeeRepo;
        _productValidator = productValidator;
        _categoryValidator = categoryValidator;
        _discountValidator = discountValidator;
        _employeeValidator = employeeValidator;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync() =>
        await _productRepo.GetAllAsync(p => p.Category, p => p.Discount);

    public async Task<Product?> GetProductByIdAsync(int id) =>
        await _productRepo.GetByIdAsync(id, p => p.Category, p => p.Discount);

    public async Task<Product> AddProductAsync(AddProductInput input)
    {
        var validationResult = _productValidator.Validate(input);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (!await _categoryRepo.ExistsAsync(input.CategoryId))
            throw new ArgumentException("Category not found");

        var product = new Product
        {
            Name = input.Name,
            Price = input.Price,
            Stock = input.Stock,
            CategoryId = input.CategoryId
        };

        await _productRepo.AddAsync(product);
        return product;
    }

    public async Task<bool> UpdateProductStockAsync(int id, int stock)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null) return false;

        if (stock < 0) throw new ArgumentException("Stock cannot be negative");

        product.Stock = stock;
        await _productRepo.UpdateAsync(product);
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null) return false;

        await _productRepo.DeleteAsync(product);
        return true;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync() =>
        await _categoryRepo.GetAllAsync();

    public async Task<Category> AddCategoryAsync(AddCategoryInput input)
    {
        var validationResult = _categoryValidator.Validate(input);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var category = new Category { Name = input.Name, Description = input.Description };
        await _categoryRepo.AddAsync(category);
        return category;
    }

    public async Task<IEnumerable<Discount>> GetAllDiscountsAsync() =>
        await _discountRepo.GetAllAsync();

    public async Task<Discount> AddDiscountAsync(AddDiscountInput input)
    {
        var validationResult = _discountValidator.Validate(input);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var discount = new Discount
        {
            Code = input.Code,
            Percentage = input.Percentage,
            ExpiresAt = input.ExpiresAt
        };
        await _discountRepo.AddAsync(discount);
        return discount;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync() =>
        await _employeeRepo.GetAllAsync();

    public async Task<Employee?> GetEmployeeByIdAsync(int id) =>
        await _employeeRepo.GetByIdAsync(id);

    public async Task<Employee> AddEmployeeAsync(AddEmployeeInput input)
    {
        var validationResult = _employeeValidator.Validate(input);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var employee = new Employee
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Role = input.Role,
            HiredAt = input.HiredAt
        };
        await _employeeRepo.AddAsync(employee);
        return employee;
    }
}
