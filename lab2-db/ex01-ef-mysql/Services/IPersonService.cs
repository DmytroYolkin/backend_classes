using Howest.lab2.ex01_ef_mysql.Models;
using Howest.lab2.ex01_ef_mysql.DTOs;

namespace Howest.lab2.ex01_ef_mysql.Services
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