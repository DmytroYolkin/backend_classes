using Howest.Lab1.Models;

namespace Howest.Lab1.Services
{
    public interface IWineService
    {
        List<Wine> GetWines();
        Wine GetWine(int id);
        void AddWine(Wine wine);
        void DeleteWine(int id);
        void UpdateWine(Wine wine);
    }
}