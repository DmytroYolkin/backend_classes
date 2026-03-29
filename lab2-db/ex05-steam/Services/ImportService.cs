using CsvHelper;
using CsvHelper.Configuration;
using Howest.ex05.Models;
using Howest.ex05.Repositories;
using System.Globalization;

namespace Howest.ex05.Services;

public class ImportService
{
    private readonly GameRepository _repo;

    public ImportService(GameRepository repo)
    {
        _repo = repo;
    }

    public async Task<TimeSpan> ImportFromFilesAsync(IFormFileCollection files)
    {
        var steamFile = files.FirstOrDefault(f => f.FileName.Contains("steam.csv"));
        var descFile = files.FirstOrDefault(f => f.FileName.Contains("description"));
        var reqFile = files.FirstOrDefault(f => f.FileName.Contains("requirements"));

        if (steamFile == null || descFile == null || reqFile == null)
            throw new ArgumentException("Missing one of the required CSV files. Ensure you upload 3 files containing 'steam.csv', 'description', and 'requirements' in their names.");

        var gamesDict = new Dictionary<int, Game>();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture) 
        { 
            HeaderValidated = null, 
            MissingFieldFound = null,
            BadDataFound = null
        };

        // 1. Parse steam.csv
        using (var stream = new StreamReader(steamFile.OpenReadStream()))
        using (var csv = new CsvReader(stream, config))
        {
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                try
                {
                    var game = new Game
                    {
                        AppId = csv.GetField<int>("appid"),
                        Name = csv.GetField("name") ?? "",
                        ReleaseDate = csv.TryGetField<DateTime>("release_date", out var d) 
                                        ? DateTime.SpecifyKind(d, DateTimeKind.Utc) 
                                        : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
                        English = csv.TryGetField<int>("english", out var e) ? e : 0,
                        Developer = csv.GetField("developer") ?? "",
                        Publisher = csv.GetField("publisher") ?? "",
                        Platforms = csv.GetField("platforms") ?? "",
                        RequiredAge = csv.TryGetField<int>("required_age", out var ra) ? ra : 0,
                        Categories = csv.GetField("categories") ?? "",
                        Genres = csv.GetField("genres") ?? "",
                        SteamSpyTags = csv.GetField("steamspy_tags") ?? "",
                        Tags = csv.GetField("steamspy_tags")?.Split(';').ToList() ?? new List<string>(),
                        Achievements = csv.TryGetField<int>("achievements", out var ach) ? ach : 0,
                        PositiveRatings = csv.TryGetField<int>("positive_ratings", out var pr) ? pr : 0,
                        NegativeRatings = csv.TryGetField<int>("negative_ratings", out var nr) ? nr : 0,
                        AveragePlaytime = csv.TryGetField<int>("average_playtime", out var ap) ? ap : 0,
                        MedianPlaytime = csv.TryGetField<int>("median_playtime", out var mp) ? mp : 0,
                        Owners = csv.GetField("owners") ?? "",
                        Price = csv.TryGetField<decimal>("price", out var p) ? p : 0m
                    };
                    gamesDict[game.AppId] = game;
                }
                catch (Exception)
                {
                    // Ignore problematic rows
                }
            }
        }

        // 2. Parse descriptions
        using (var stream = new StreamReader(descFile.OpenReadStream()))
        using (var csv = new CsvReader(stream, config))
        {
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                try 
                {
                    if (csv.TryGetField<int>("steam_appid", out int appId) && gamesDict.TryGetValue(appId, out var game))
                    {
                        game.DetailedDescription = csv.GetField("detailed_description") ?? "";
                        game.AboutTheGame = csv.GetField("about_the_game") ?? "";
                        game.ShortDescription = csv.GetField("short_description") ?? "";
                    }
                } 
                catch {}
            }
        }

        // 3. Parse requirements
        using (var stream = new StreamReader(reqFile.OpenReadStream()))
        using (var csv = new CsvReader(stream, config))
        {
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                try 
                {
                    if (csv.TryGetField<int>("steam_appid", out int appId) && gamesDict.TryGetValue(appId, out var game))
                    {
                        game.Requirements.Minimum = csv.GetField("minimum") ?? "";
                        game.Requirements.Recommended = csv.GetField("recommended") ?? "";
                        game.Requirements.PcRequirements = csv.GetField("pc_requirements") ?? "";
                        game.Requirements.MacRequirements = csv.GetField("mac_requirements") ?? "";
                        game.Requirements.LinuxRequirements = csv.GetField("linux_requirements") ?? "";
                    }
                } 
                catch {}
            }
        }

        // Import the combined records
        return await _repo.ImportGamesAsync(gamesDict.Values);
    }
}