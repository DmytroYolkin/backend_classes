using ex07.DTOs;
using ex07.Services;
using FluentValidation;

namespace ex07.Endpoints;

public static class WorkoutEndpoints
{
    public static void MapWorkoutEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v{version:apiVersion}/workouts").WithTags("Workouts");

        group.MapGet("/", async (IWorkoutService service) => Results.Ok(await service.GetAllAsync()));

        group.MapGet("/{id:guid}", async (Guid id, IWorkoutService service) => Results.Ok(await service.GetByIdAsync(id)));

        group.MapPost("/", async (CreateWorkoutDto dto, IWorkoutService service) =>
        {
            try
            {
                var created = await service.CreateAsync(dto);
                return Results.Created($"/api/v1/workouts/{created.Id}", created);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(ex.Errors);
            }
        });

        group.MapDelete("/{id:guid}", async (Guid id, IWorkoutService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}
