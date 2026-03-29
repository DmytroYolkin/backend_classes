namespace ex03_ef_postgresql.Models;

public class Traveler
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public Passport? Passport { get; set; }
    public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
}