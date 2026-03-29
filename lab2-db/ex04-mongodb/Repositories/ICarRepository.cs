namespace Howest.Lab2.Ex04.Repositories;

public interface ICarRepository
{
    Task AddCar(Car car);
    Task<Car> GetCar(string id);
    Task<IEnumerable<Car>> GetAllCars();
}