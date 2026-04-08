namespace ex03_ef_postgresql.Models;

public class Guide
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Tour> Tours { get; set; } = new List<Tour>();
}