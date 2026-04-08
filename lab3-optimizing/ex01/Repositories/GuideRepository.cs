using ex03_ef_postgresql.Data;
using ex03_ef_postgresql.Models;
using Microsoft.EntityFrameworkCore;

namespace ex03_ef_postgresql.Repositories;

public class GuideRepository : IGuideRepository
{
    private readonly TravelDbContext _context;

    public GuideRepository(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<List<Guide>> GetAllAsync(bool includeTours = false)
    {
        var query = _context.Guides.AsQueryable();

        if (includeTours)
        {
            query = query.Include(g => g.Tours);
        }

        return await query.ToListAsync();
    }

    public async Task<Guide?> GetByIdAsync(int id, bool includeTours = false)
    {
        var query = _context.Guides.AsQueryable();

        if (includeTours)
        {
            query = query.Include(g => g.Tours);
        }

        return await query.FirstOrDefaultAsync(g => g.Id == id);
    }
}
