using ex07.Data;
using ex07.Models;
using Microsoft.EntityFrameworkCore;

namespace ex07.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly AppDbContext _context;

    public WorkoutRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Workout>> GetAllAsync()
    {
        return await _context.Workouts.ToListAsync();
    }

    public async Task<Workout?> GetByIdAsync(Guid id)
    {
        return await _context.Workouts.FindAsync(id);
    }

    public async Task<Workout> AddAsync(Workout workout)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();
        return workout;
    }

    public async Task DeleteAsync(Workout workout)
    {
        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync();
    }
}
