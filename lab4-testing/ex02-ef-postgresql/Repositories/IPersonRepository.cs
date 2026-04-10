using Howest.lab2.ex02_ef_postgresql.Models;

namespace Howest.lab2.ex02_ef_postgresql.Repositories
{
    public interface IPersonRepository
    {
        Person AddPerson(Person person);
        Person UpdatePerson(int id, Person person);
        void DeletePerson(int id);
        Person GetPerson(int id);
        IEnumerable<Person> GetAllPersons();
    }
}
