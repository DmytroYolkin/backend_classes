namespace ex03_ef_postgresql.Models;

public class Passport
{
    public int Id { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public int TravelerId { get; set; }
    public Traveler? Traveler { get; set; }
}