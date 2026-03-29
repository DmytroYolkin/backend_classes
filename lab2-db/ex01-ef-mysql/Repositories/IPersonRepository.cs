using Howest.lab2.ex01_ef_mysql.Models;

namespace Howest.lab2.ex01_ef_mysql.Repositories
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