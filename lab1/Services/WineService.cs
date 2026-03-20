using Howest.Lab1.Models;
using Howest.Lab1.Repositories;

namespace Howest.Lab1.Services
{
    public class WineService : IWineService
    {
        private readonly IWineRepository _wineRepository;

        public WineService(IWineRepository wineRepository)
        {
            _wineRepository = wineRepository;
        }

        public List<Wine> GetWines()
        {
            return _wineRepository.GetWines();
        }

        public Wine GetWine(int id)
        {
            return _wineRepository.GetWine(id);
        }

        public void AddWine(Wine wine)
        {
            _wineRepository.AddWine(wine);
        }

        public void DeleteWine(int id)
        {
            _wineRepository.DeleteWine(id);
        }

        public void UpdateWine(Wine wine)
        {
            _wineRepository.UpdateWine(wine);
        }
    }
}