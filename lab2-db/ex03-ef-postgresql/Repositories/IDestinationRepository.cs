using ex03_ef_postgresql.Models;

namespace ex03_ef_postgresql.Repositories;

public interface IDestinationRepository
{
    Task<List<Destination>> GetAllAsync();
    Task<Destination?> GetByIdAsync(int id);
    Task AddAsync(Destination destination);
}
