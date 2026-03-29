using ex03_ef_postgresql.Data;
using ex03_ef_postgresql.Models;
using Microsoft.EntityFrameworkCore;

namespace ex03_ef_postgresql.Repositories;

public class TravelerRepository : ITravelerRepository
{
    private readonly TravelDbContext _context;

    public TravelerRepository(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<List<Traveler>> GetAllAsync()
    {
        return await _context.Travelers
            .Include(t => t.Passport)
            .Include(t => t.Destinations)
            .ToListAsync();
    }

    public async Task<Traveler?> GetByIdAsync(int id)
    {
        return await _context.Travelers
            .Include(t => t.Passport)
            .Include(t => t.Destinations)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> ExistsByPassportNumberAsync(string passportNumber)
    {
        return await _context.Passports.AnyAsync(p => p.PassportNumber == passportNumber);
    }

    public async Task AddAsync(Traveler traveler)
    {
        await _context.Travelers.AddAsync(traveler);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Traveler traveler)
    {
        _context.Travelers.Update(traveler);
        await _context.SaveChangesAsync();
    }
}
