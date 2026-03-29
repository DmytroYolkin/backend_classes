using Howest.ex05.Data;
using Howest.ex05.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Howest.ex05.Repositories;

public class GameRepository 
{
    private readonly SteamContext _context;

    public GameRepository(SteamContext context)
    {
        _context = context;
    }

    public async Task<TimeSpan> ImportGamesAsync(IEnumerable<Game> games)
    {
        var sw = Stopwatch.StartNew();
        
        // EF Core bulk adds are greatly optimized in Npgsql
        // Disable AutoDetectChanges to significantly speed up insert
        _context.ChangeTracker.AutoDetectChangesEnabled = false;
        try
        {
            await _context.Games.AddRangeAsync(games);
            await _context.SaveChangesAsync();
        }
        finally
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = true;
        }
        
        sw.Stop();
        return sw.Elapsed;
    }

    public async Task<List<Game>> SearchFullTextAsync(string query)
    {
        return await _context.Games
            .Where(g => g.SearchVector.Matches(query)) // Built-in Npgsql operator for tsvector
            .ToListAsync();
    }

    public async Task<List<Game>> SearchTagsAsync(string tagName)
    {
        return await _context.Games
            .Where(g => g.Tags.Contains(tagName)) // Translated to JSONB query inside PostgreSQL
            .ToListAsync();
    }

    public async Task<List<Game>> SearchRequirementsAsync(string query)
    {
        return await _context.Games
            .Where(g => g.Requirements.Minimum.Contains(query) || g.Requirements.Recommended.Contains(query))
            .ToListAsync();
    }
}