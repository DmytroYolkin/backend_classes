using Howest.lab2.ex02_ef_postgresql.Data;
using Howest.lab2.ex02_ef_postgresql.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.lab2.ex02_ef_postgresql.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonContext _context;

        public PersonRepository(PersonContext context)
        {
            _context = context;
        }

        public Person AddPerson(Person person)
        {
            _context.People.Add(person);
            _context.SaveChanges();
            return person;
        }

        public Person UpdatePerson(int id, Person person)
        {
            var existingPerson = _context.People.Find(id);
            if (existingPerson != null)
            {
                existingPerson.Name = person.Name;
                existingPerson.Age = person.Age;
                existingPerson.Email = person.Email;
                _context.SaveChanges();
            }
            return existingPerson;
        }

        public void DeletePerson(int id)
        {
            var person = _context.People.Find(id);
            if (person != null)
            {
                _context.People.Remove(person);
                _context.SaveChanges();
            }
        }

        public Person GetPerson(int id)
        {
            return _context.People.Find(id);
        }

        public IEnumerable<Person> GetAllPersons()
        {
            return _context.People.ToList();
        }
    }
}
