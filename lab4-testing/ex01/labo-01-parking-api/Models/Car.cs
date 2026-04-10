namespace labo_01_parking_api.Models;

public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}