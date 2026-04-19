using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStoreAPI.Data;
using RetailStoreAPI.Models;
using RetailStoreAPI.DTOs;
using RetailStoreAPI.Services;

namespace RetailStoreAPI.Endpoints;

public static class StoreEndpoints
{
    public static void MapStoreEndpoints(this IEndpointRouteBuilder app)
    {
        var products = app.MapGroup("/api/products");
        
        products.MapGet("/", async (IStoreService service) => 
            await service.GetAllProductsAsync());

        products.MapGet("/{id}", async (int id, IStoreService service) => 
            await service.GetProductByIdAsync(id) is Product product 
                ? Results.Ok(product) 
                : Results.NotFound());

        products.MapPost("/", async (AddProductInput input, IStoreService service) =>
        {
            try
            {
                var product = await service.AddProductAsync(input);
                return Results.Created($"/api/products/{product.Id}", product);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(ex.Errors);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        products.MapPut("/{id}", async (int id, UpdateProductStockInput input, IStoreService service) =>
        {
            try
            {
                var success = await service.UpdateProductStockAsync(id, input.Stock);
                return success ? Results.NoContent() : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        products.MapDelete("/{id}", async (int id, IStoreService service) =>
        {
            var success = await service.DeleteProductAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });

        var categories = app.MapGroup("/api/categories");
        
        categories.MapGet("/", async (IStoreService service) => await service.GetAllCategoriesAsync());
        
        categories.MapPost("/", async (AddCategoryInput input, IStoreService service) => 
        {
            try
            {
                var category = await service.AddCategoryAsync(input);
                return Results.Created($"/api/categories/{category.Id}", category);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(ex.Errors);
            }
        });

        var discounts = app.MapGroup("/api/discounts");
        
        discounts.MapGet("/", async (IStoreService service) => await service.GetAllDiscountsAsync());
        
        discounts.MapPost("/", async (AddDiscountInput input, IStoreService service) => 
        {
            try
            {
                var discount = await service.AddDiscountAsync(input);
                return Results.Created($"/api/discounts/{discount.Id}", discount);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(ex.Errors);
            }
        });

        var employees = app.MapGroup("/api/employees");
        
        employees.MapGet("/", async (IStoreService service) => await service.GetAllEmployeesAsync());
        
        employees.MapGet("/{id}", async (int id, IStoreService service) => 
            await service.GetEmployeeByIdAsync(id) is Employee employee ? Results.Ok(employee) : Results.NotFound());

        employees.MapPost("/", async (AddEmployeeInput input, IStoreService service) => 
        {
            try
            {
                var employee = await service.AddEmployeeAsync(input);
                return Results.Created($"/api/employees/{employee.Id}", employee);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(ex.Errors);
            }
        });
    }
}
