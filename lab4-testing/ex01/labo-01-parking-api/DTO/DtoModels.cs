namespace labo_01_parking_api.DTO;

public record CreateCarDto(string Brand, string Model, string Plate, string Color);

public record CarDto(int Id, string Brand, string Model, string Plate, string Color);

public record CreateRegistrationDto(string Plate, int CarId);

public record UpdateRegistrationDto(int Id, int CarId);

public record RegistrationDto(int Id, string Plate, DateTime Start, DateTime? End, int CarId, decimal TotalPrice, bool IsFinished);

public record RegistrationResponseDto(int Id, string Plate, DateTime? End, decimal TotalPrice, bool IsFinished);
