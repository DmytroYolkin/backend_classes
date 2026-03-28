namespace ex03_ef_postgresql.DTOs;

public record TravelerDto(int TravelerId, string FullName, string? PassportNumber);
public record TravelerDetailDto(int TravelerId, string FullName, string? PassportNumber, List<DestinationDto> Destinations);
public record TravelerInputDto(string FullName, string PassportNumber);
