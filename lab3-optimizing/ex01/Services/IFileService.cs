namespace ex03_ef_postgresql.Services;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);
    Task<(byte[] Content, string FileName)> ExportDatabaseToCsvAsync();
}