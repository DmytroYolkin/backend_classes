namespace ex03_ef_postgresql.Models;

public class Destination
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Traveler> Travelers { get; set; } = new List<Traveler>();
}