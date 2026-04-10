namespace labo_01_parking_api.Models;

public class Registration
{
    public int Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public int CarId { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsFinished { get; set; }
}