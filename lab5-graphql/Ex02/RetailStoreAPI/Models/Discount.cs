namespace RetailStoreAPI.Models;

public class Discount
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public double Percentage { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
