namespace ex03_ef_postgresql.DTOs;

public record DestinationDto(int DestinationId, string Name);

public record DestinationV2Dto(int DestinationId, string Name, double AveragePrice);
