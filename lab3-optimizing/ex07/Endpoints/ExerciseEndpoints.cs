using ex07.DTOs;
using ex07.Services;
using FluentValidation;

namespace ex07.Endpoints;

public static class ExerciseEndpoints
{
    public static void MapExerciseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v{version:apiVersion}/exercises").WithTags("Exercises");

        group.MapGet("/", async (IExerciseService service) => Results.Ok(await service.GetAllAsync()));

        group.MapGet("/{id:guid}", async (Guid id, IExerciseService service) => Results.Ok(await service.GetByIdAsync(id)));

        group.MapPost("/", async (CreateExerciseDto dto, IExerciseService service) =>
        {
            try
            {
                var created = await service.CreateAsync(dto);
                return Results.Created($"/api/v1/exercises/{created.Id}", created);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(ex.Errors);
            }
        });

        group.MapDelete("/{id:guid}", async (Guid id, IExerciseService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}
