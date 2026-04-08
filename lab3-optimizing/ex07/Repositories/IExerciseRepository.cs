using ex07.Models;

namespace ex07.Repositories;

public interface IExerciseRepository
{
    Task<List<Exercise>> GetAllAsync();
    Task<Exercise?> GetByIdAsync(Guid id);
    Task<Exercise> AddAsync(Exercise exercise);
    Task UpdateAsync(Exercise exercise);
    Task DeleteAsync(Exercise exercise);
}
