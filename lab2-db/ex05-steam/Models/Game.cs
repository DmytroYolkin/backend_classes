namespace Howest.ex05.Models;
public class Game
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int AppId { get; set; }
    
    [Column(TypeName = "jsonb")]
    public List<string> Tags { get; set; } = new(); // Parsed from SteamSpyTags

    // Flattened / JSONB Requirements
    public SystemRequirements Requirements { get; set; } = new();

    // Full Text Search Vector
    public NpgsqlTsVector SearchVector { get; set; } = null!;

    public required string Name { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int English { get; set; }
    public required string Developer { get; set; }
    public required string Publisher { get; set; }
    public required string Platforms { get; set; }
    public int RequiredAge { get; set; }
    public required string Categories { get; set; }
    public required string Genres { get; set; }
    public required string SteamSpyTags { get; set; } // Keep original string if needed, or remove? Keeping for completeness


    public int Achievements { get; set; }
    public int PositiveRatings { get; set; }
    public int NegativeRatings { get; set; }
    public int AveragePlaytime { get; set; }
    public int MedianPlaytime { get; set; }
    public required string Owners { get; set; }
    public decimal Price { get; set; }

    // Flattened Description
    public string DetailedDescription { get; set; } = string.Empty;
    public string AboutTheGame { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;



}

public class SystemRequirements
{
    public string Minimum { get; set; } = "N/A";
    public string Recommended { get; set; } = "N/A";
    public string PcRequirements { get; set; } = string.Empty;
    public string MacRequirements { get; set; } = string.Empty;
    public string LinuxRequirements { get; set; } = string.Empty;
}

