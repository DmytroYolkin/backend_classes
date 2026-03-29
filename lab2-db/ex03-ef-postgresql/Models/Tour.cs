namespace ex03_ef_postgresql.Models;

public class Tour
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int GuideId { get; set; }
    public Guide? Guide { get; set; }
}