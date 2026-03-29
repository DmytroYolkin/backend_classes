using ex03_ef_postgresql.DTOs;

namespace ex03_ef_postgresql.Services;

public interface ITravelerService
{
    Task<List<TravelerDto>> GetAllTravelersAsync();
    Task<TravelerDto?> GetTravelerByIdAsync(int id);
    Task<TravelerDto> AddTravelerAsync(TravelerInputDto input);
    Task<TravelerDetailDto?> AddTravelerToDestinationAsync(int travelerId, int destinationId);
}
