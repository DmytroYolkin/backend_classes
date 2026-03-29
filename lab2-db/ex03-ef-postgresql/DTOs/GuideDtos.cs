namespace ex03_ef_postgresql.DTOs;

public record GuideDto(int GuideId, string Name);
public record GuideWithToursDto(int GuideId, string Name, List<TourDto> Tours);
public record TourDto(int TourId, string Title);
