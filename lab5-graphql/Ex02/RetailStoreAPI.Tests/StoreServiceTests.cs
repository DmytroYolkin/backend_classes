using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using RetailStoreAPI.DTOs;
using RetailStoreAPI.Models;
using RetailStoreAPI.Repositories;
using RetailStoreAPI.Services;
using System.Linq.Expressions;
using Xunit;

namespace RetailStoreAPI.Tests;

public class StoreServiceTests
{
    private readonly Mock<IRepository<Product>> _productRepoMock;
    private readonly Mock<IRepository<Category>> _categoryRepoMock;
    private readonly Mock<IRepository<Discount>> _discountRepoMock;
    private readonly Mock<IRepository<Employee>> _employeeRepoMock;
    private readonly Mock<IValidator<AddProductInput>> _productValidatorMock;
    private readonly Mock<IValidator<AddCategoryInput>> _categoryValidatorMock;
    private readonly Mock<IValidator<AddDiscountInput>> _discountValidatorMock;
    private readonly Mock<IValidator<AddEmployeeInput>> _employeeValidatorMock;
    private readonly StoreService _service;

    public StoreServiceTests()
    {
        _productRepoMock = new Mock<IRepository<Product>>();
        _categoryRepoMock = new Mock<IRepository<Category>>();
        _discountRepoMock = new Mock<IRepository<Discount>>();
        _employeeRepoMock = new Mock<IRepository<Employee>>();
        
        _productValidatorMock = new Mock<IValidator<AddProductInput>>();
        _categoryValidatorMock = new Mock<IValidator<AddCategoryInput>>();
        _discountValidatorMock = new Mock<IValidator<AddDiscountInput>>();
        _employeeValidatorMock = new Mock<IValidator<AddEmployeeInput>>();

        _service = new StoreService(
            _productRepoMock.Object,
            _categoryRepoMock.Object,
            _discountRepoMock.Object,
            _employeeRepoMock.Object,
            _productValidatorMock.Object,
            _categoryValidatorMock.Object,
            _discountValidatorMock.Object,
            _employeeValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<Product> { new Product { Id = 1, Name = "P1" } };
        _productRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Product, object?>>[]>()))
            .ReturnsAsync(products);

        // Act
        var result = await _service.GetAllProductsAsync();

        // Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsProduct_WhenExists()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "P1" };
        _productRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<Expression<Func<Product, object?>>[]>()))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetProductByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task AddProductAsync_ThrowsValidationException_WhenInvalid()
    {
        // Arrange
        var input = new AddProductInput("", 0, 0, 0);
        var validationFailures = new List<ValidationFailure> { new ValidationFailure("Name", "Required") };
        _productValidatorMock.Setup(v => v.Validate(input)).Returns(new ValidationResult(validationFailures));

        // Act & Assert
        await _service.Invoking(s => s.AddProductAsync(input))
            .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AddProductAsync_ThrowsArgumentException_WhenCategoryNotFound()
    {
        // Arrange
        var input = new AddProductInput("P1", 10, 5, 99);
        _productValidatorMock.Setup(v => v.Validate(input)).Returns(new ValidationResult());
        _categoryRepoMock.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        // Act & Assert
        await _service.Invoking(s => s.AddProductAsync(input))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Category not found");
    }

    [Fact]
    public async Task AddProductAsync_AddsAndReturnsProduct_WhenValid()
    {
        // Arrange
        var input = new AddProductInput("P1", 10, 5, 1);
        _productValidatorMock.Setup(v => v.Validate(input)).Returns(new ValidationResult());
        _categoryRepoMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.AddProductAsync(input);

        // Assert
        result.Name.Should().Be(input.Name);
        _productRepoMock.Verify(r => r.AddAsync(It.Is<Product>(p => p.Name == "P1")), Times.Once);
    }

    [Fact]
    public async Task UpdateProductStockAsync_ReturnsFalse_WhenProductNotFound()
    {
        // Arrange
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        // Act
        var result = await _service.UpdateProductStockAsync(1, 10);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateProductStockAsync_ThrowsArgumentException_WhenStockIsNegative()
    {
        // Arrange
        var product = new Product { Id = 1, Stock = 5 };
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        // Act & Assert
        await _service.Invoking(s => s.UpdateProductStockAsync(1, -1))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Stock cannot be negative");
    }

    [Fact]
    public async Task UpdateProductStockAsync_UpdatesStock_WhenValid()
    {
        // Arrange
        var product = new Product { Id = 1, Stock = 5 };
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _service.UpdateProductStockAsync(1, 10);

        // Assert
        result.Should().BeTrue();
        product.Stock.Should().Be(10);
        _productRepoMock.Verify(r => r.UpdateAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_ReturnsFalse_WhenNotFound()
    {
        // Arrange
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        // Act
        var result = await _service.DeleteProductAsync(1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteProductAsync_Deletes_WhenExists()
    {
        // Arrange
        var product = new Product { Id = 1 };
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _service.DeleteProductAsync(1);

        // Assert
        result.Should().BeTrue();
        _productRepoMock.Verify(r => r.DeleteAsync(product), Times.Once);
    }

    [Fact]
    public async Task AddCategoryAsync_AddsAndReturnsCategory_WhenValid()
    {
        // Arrange
        var input = new AddCategoryInput("C1", "D1");
        _categoryValidatorMock.Setup(v => v.Validate(input)).Returns(new ValidationResult());

        // Act
        var result = await _service.AddCategoryAsync(input);

        // Assert
        result.Name.Should().Be("C1");
        _categoryRepoMock.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task AddDiscountAsync_AddsAndReturnsDiscount_WhenValid()
    {
        // Arrange
        var input = new AddDiscountInput("SAVE10", 10, null);
        _discountValidatorMock.Setup(v => v.Validate(input)).Returns(new ValidationResult());

        // Act
        var result = await _service.AddDiscountAsync(input);

        // Assert
        result.Code.Should().Be("SAVE10");
        _discountRepoMock.Verify(r => r.AddAsync(It.IsAny<Discount>()), Times.Once);
    }

    [Fact]
    public async Task AddEmployeeAsync_AddsAndReturnsEmployee_WhenValid()
    {
        // Arrange
        var input = new AddEmployeeInput("John", "Doe", "Manager", DateTime.Now);
        _employeeValidatorMock.Setup(v => v.Validate(input)).Returns(new ValidationResult());

        // Act
        var result = await _service.AddEmployeeAsync(input);

        // Assert
        result.FirstName.Should().Be("John");
        _employeeRepoMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
    }
}
