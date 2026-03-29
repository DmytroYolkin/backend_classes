namespace Howest.Lab2.Ex04.Repositories;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetAllBrands();
    Task AddBrand(Brand newBrand);
    Task<Brand> GetBrand(string id);
    Task UpdateBrand(Brand brand);
}