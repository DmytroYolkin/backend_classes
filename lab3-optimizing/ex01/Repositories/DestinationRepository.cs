using ex03_ef_postgresql.Data;
using ex03_ef_postgresql.Models;
using Microsoft.EntityFrameworkCore;

namespace ex03_ef_postgresql.Repositories;

public class DestinationRepository : IDestinationRepository
{
    private readonly TravelDbContext _context;

    public DestinationRepository(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<List<Destination>> GetAllAsync()
    {
        return await _context.Destinations.ToListAsync();
    }

    public async Task<Destination?> GetByIdAsync(int id)
    {
        return await _context.Destinations.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Destination destination)
    {
        await _context.Destinations.AddAsync(destination);
        await _context.SaveChangesAsync();
    }
}
