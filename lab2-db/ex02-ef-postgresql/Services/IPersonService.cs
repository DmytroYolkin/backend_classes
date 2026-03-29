using Howest.lab2.ex02_ef_postgresql.Models;
using Howest.lab2.ex02_ef_postgresql.DTOs;

namespace Howest.lab2.ex02_ef_postgresql.Services
{
    public interface IPersonService
    {
        PersonDTO AddPerson(PersonDTO personDto);
        PersonDTO? UpdatePerson(int id, PersonDTO personDto);
        void DeletePerson(int id);
        PersonDTO? GetPerson(int id);
        IEnumerable<PersonDTO> GetAllPersons();
    }
}
