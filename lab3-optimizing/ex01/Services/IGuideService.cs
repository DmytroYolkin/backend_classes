using ex03_ef_postgresql.DTOs;

namespace ex03_ef_postgresql.Services;

public interface IGuideService
{
    Task<List<object>> GetAllGuidesAsync(bool includeTours);
    Task<object?> GetGuideByIdAsync(int id, bool includeTours);
}
