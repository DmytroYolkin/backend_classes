using ex03_ef_postgresql.Models;

namespace ex03_ef_postgresql.Repositories;

public interface IGuideRepository
{
    Task<List<Guide>> GetAllAsync(bool includeTours = false);
    Task<Guide?> GetByIdAsync(int id, bool includeTours = false);
}
