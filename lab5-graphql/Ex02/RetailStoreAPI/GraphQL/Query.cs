using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using RetailStoreAPI.Data;
using RetailStoreAPI.Models;

namespace RetailStoreAPI.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Product> GetProducts([Service] AppDbContext context) =>
        context.Products.Include(p => p.Category).Include(p => p.Discount);

    public async Task<Product?> GetProductAsync(int id, [Service] AppDbContext context) =>
        await context.Products.Include(p => p.Category).Include(p => p.Discount)
            .FirstOrDefaultAsync(p => p.Id == id);

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Category> GetCategories([Service] AppDbContext context) =>
        context.Categories.Include(c => c.Products);

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Discount> GetDiscounts([Service] AppDbContext context) =>
        context.Discounts;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Employee> GetEmployees([Service] AppDbContext context) =>
        context.Employees;
}
