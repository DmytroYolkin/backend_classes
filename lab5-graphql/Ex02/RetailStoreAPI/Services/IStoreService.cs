using RetailStoreAPI.Models;
using RetailStoreAPI.Repositories;
using RetailStoreAPI.DTOs;
using FluentValidation;

namespace RetailStoreAPI.Services;

public interface IStoreService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> AddProductAsync(AddProductInput input);
    Task<bool> UpdateProductStockAsync(int id, int stock);
    Task<bool> DeleteProductAsync(int id);

    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> AddCategoryAsync(AddCategoryInput input);

    Task<IEnumerable<Discount>> GetAllDiscountsAsync();
    Task<Discount> AddDiscountAsync(AddDiscountInput input);

    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<Employee> AddEmployeeAsync(AddEmployeeInput input);
}
