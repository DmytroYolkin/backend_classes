using ex03_ef_postgresql.Models;

namespace ex03_ef_postgresql.Repositories;

public interface ITravelerRepository
{
    Task<List<Traveler>> GetAllAsync();
    Task<Traveler?> GetByIdAsync(int id);
    Task<bool> ExistsByPassportNumberAsync(string passportNumber);
    Task AddAsync(Traveler traveler);
    Task UpdateAsync(Traveler traveler);
}
