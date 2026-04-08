using ex07.Models;

namespace ex07.Repositories;

public interface IWorkoutRepository
{
    Task<List<Workout>> GetAllAsync();
    Task<Workout?> GetByIdAsync(Guid id);
    Task<Workout> AddAsync(Workout workout);
    Task DeleteAsync(Workout workout);
}
