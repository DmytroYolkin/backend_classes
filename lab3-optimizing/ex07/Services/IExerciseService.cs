using ex07.DTOs;

namespace ex07.Services;

public interface IExerciseService
{
    Task<List<ExerciseDto>> GetAllAsync();
    Task<ExerciseDto> GetByIdAsync(Guid id);
    Task<ExerciseDto> CreateAsync(CreateExerciseDto dto);
    Task DeleteAsync(Guid id);
}
