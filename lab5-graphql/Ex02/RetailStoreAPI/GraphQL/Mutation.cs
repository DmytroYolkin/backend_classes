using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStoreAPI.Data;
using RetailStoreAPI.Models;
using RetailStoreAPI.DTOs;

namespace RetailStoreAPI.GraphQL;

public class Mutation
{
    public async Task<Product> AddProductAsync(AddProductInput input, [Service] AppDbContext context, [Service] IValidator<AddProductInput> validator)
    {
        await validator.ValidateAndThrowAsync(input);
        
        if (!await context.Categories.AnyAsync(c => c.Id == input.CategoryId))
            throw new GraphQLException("Category not found");

        var product = new Product { Name = input.Name, Price = input.Price, Stock = input.Stock, CategoryId = input.CategoryId };
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateStockAsync(int id, int stock, [Service] AppDbContext context)
    {
        var product = await context.Products.FindAsync(id) ?? throw new GraphQLException("Product not found");
        if (stock < 0) throw new GraphQLException("Stock cannot be negative");
        
        product.Stock = stock;
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> ApplyDiscountAsync(int productId, int discountId, [Service] AppDbContext context)
    {
        var product = await context.Products.FindAsync(productId) ?? throw new GraphQLException("Product not found");
        if (!await context.Discounts.AnyAsync(d => d.Id == discountId)) throw new GraphQLException("Discount not found");

        product.DiscountId = discountId;
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Discount> AddDiscountAsync(AddDiscountInput input, [Service] AppDbContext context, [Service] IValidator<AddDiscountInput> validator)
    {
         await validator.ValidateAndThrowAsync(input);
         var discount = new Discount { Code = input.Code, Percentage = input.Percentage, ExpiresAt = input.ExpiresAt };
         context.Discounts.Add(discount);
         await context.SaveChangesAsync();
         return discount;
    }

    public async Task<Employee> AddEmployeeAsync(AddEmployeeInput input, [Service] AppDbContext context, [Service] IValidator<AddEmployeeInput> validator)
    {
         await validator.ValidateAndThrowAsync(input);
         var employee = new Employee { FirstName = input.FirstName, LastName = input.LastName, Role = input.Role, HiredAt = input.HiredAt };
         context.Employees.Add(employee);
         await context.SaveChangesAsync();
         return employee;
    }
}
