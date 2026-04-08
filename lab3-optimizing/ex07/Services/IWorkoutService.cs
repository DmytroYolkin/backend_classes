using ex07.DTOs;

namespace ex07.Services;

public interface IWorkoutService
{
    Task<List<WorkoutDto>> GetAllAsync();
    Task<WorkoutDto> GetByIdAsync(Guid id);
    Task<WorkoutDto> CreateAsync(CreateWorkoutDto dto);
    Task DeleteAsync(Guid id);
}
