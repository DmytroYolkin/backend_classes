namespace RetailStoreAPI.DTOs;

public record AddProductInput(string Name, decimal Price, int Stock, int CategoryId);
public record UpdateProductStockInput(int Stock);
public record AddCategoryInput(string Name, string? Description);
public record AddDiscountInput(string Code, double Percentage, DateTime? ExpiresAt);
public record AddEmployeeInput(string FirstName, string LastName, string Role, DateTime HiredAt);
