using ex03_ef_postgresql.DTOs;

namespace ex03_ef_postgresql.Services;

public interface IDestinationService
{
    Task<List<DestinationDto>> GetAllDestinationsAsync();
    Task<DestinationDto> AddDestinationAsync(DestinationDto input);
}
