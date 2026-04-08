using ex03_ef_postgresql.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ex03_ef_postgresql.Services;

public class FileService : IFileService
{
    private readonly TravelDbContext _db;
    private readonly string _uploadPath;

    public FileService(TravelDbContext db, IWebHostEnvironment environment)
    {
        _db = db;
        _uploadPath = Path.Combine(environment.ContentRootPath, "uploads");
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file uploaded");

        if (Path.GetExtension(file.FileName).ToLower() != ".csv")
            throw new ArgumentException("Only CSV files are allowed");

        var filePath = Path.Combine(_uploadPath, file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return filePath;
    }

    public async Task<(byte[] Content, string FileName)> ExportDatabaseToCsvAsync()
    {
        var travelers = await _db.Travelers.Include(t => t.Passport).ToListAsync();
        var destinations = await _db.Destinations.ToListAsync();
        var guides = await _db.Guides.ToListAsync();

        var sb = new StringBuilder();

        sb.AppendLine("TRAVELERS");
        sb.AppendLine("Id,FullName,PassportNumber");
        foreach (var t in travelers)
        {
            sb.AppendLine($"{t.Id},{t.FullName},{t.Passport?.PassportNumber}");
        }

        sb.AppendLine();
        sb.AppendLine("DESTINATIONS");
        sb.AppendLine("Id,Name");
        foreach (var d in destinations)
        {
            sb.AppendLine($"{d.Id},{d.Name}");
        }

        sb.AppendLine();
        sb.AppendLine("GUIDES");
        sb.AppendLine("Id,Name");
        foreach (var g in guides)
        {
            sb.AppendLine($"{g.Id},{g.Name}");
        }

        var fileName = $"export_{DateTime.Now:yyyyMMddHHmmss}.csv";
        return (Encoding.UTF8.GetBytes(sb.ToString()), fileName);
    }
}