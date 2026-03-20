using CsvHelper;
using CsvHelper.Configuration;
using Howest.Lab1.Models;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Howest.Lab1.Repositories
{
    public class WineRepository : IWineRepository
    {
        private readonly string _filePath;

        public WineRepository(IConfiguration configuration)
        {
            _filePath = configuration.GetValue<string>("CsvFilePath") ?? "Data/wines.csv";
        }

        public List<Wine> GetWines()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Wine>();
            }

            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Wine>().ToList();
            }
        }

        public Wine GetWine(int id)
        {
            return GetWines().FirstOrDefault(w => w.WineID == id);
        }

        public void AddWine(Wine wine)
        {
            var wines = GetWines();
            wine.WineID = wines.Any() ? wines.Max(w => w.WineID) + 1 : 1;
            wines.Add(wine);
            SaveWines(wines);
        }

        public void DeleteWine(int id)
        {
            var wines = GetWines();
            var wineToRemove = wines.FirstOrDefault(w => w.WineID == id);
            if (wineToRemove != null)
            {
                wines.Remove(wineToRemove);
                SaveWines(wines);
            }
        }

        public void UpdateWine(Wine wine)
        {
            var wines = GetWines();
            var existingWine = wines.FirstOrDefault(w => w.WineID == wine.WineID);
            if (existingWine != null)
            {
                existingWine.Name = wine.Name;
                existingWine.Year = wine.Year;
                existingWine.Country = wine.Country;
                existingWine.Color = wine.Color;
                existingWine.Price = wine.Price;
                existingWine.Grapes = wine.Grapes;
                SaveWines(wines);
            }
        }

        private void SaveWines(List<Wine> wines)
        {
            // Ensure directory exists
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(wines);
            }
        }
    }
}