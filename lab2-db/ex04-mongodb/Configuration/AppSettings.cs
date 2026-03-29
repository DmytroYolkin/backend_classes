namespace Howest.Lab2.Ex04.Configuration;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string CarsCollection { get; set; } = null!;
    public string BrandsCollection { get; set; } = null!;
}