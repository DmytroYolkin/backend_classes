namespace Howest.Lab2.Ex04.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly IMongoContext _context;

    public BrandRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task AddBrand(Brand newBrand)
    {
        await _context.BrandsCollection.InsertOneAsync(newBrand);
    }

    public async Task<IEnumerable<Brand>> GetAllBrands()
    {
        return await _context.BrandsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Brand> GetBrand(string id)
    {
        return await _context.BrandsCollection.Find(b => b.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateBrand(Brand brand)
    {
        await _context.BrandsCollection.ReplaceOneAsync(b => b.Id == brand.Id, brand);
    }
}