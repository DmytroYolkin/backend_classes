using Howest.lab2.ex01_ef_mysql.Models;
using Howest.lab2.ex01_ef_mysql.DTOs;
using Howest.lab2.ex01_ef_mysql.Repositories;
using AutoMapper;

namespace Howest.lab2.ex01_ef_mysql.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public PersonDTO AddPerson(PersonDTO personDto)
        {
            var person = _mapper.Map<Person>(personDto);
            return _mapper.Map<PersonDTO>(_personRepository.AddPerson(person));
        }

        public PersonDTO? UpdatePerson(int id, PersonDTO personDto)
        {
            var person = _mapper.Map<Person>(personDto);
            var updatedPerson = _personRepository.UpdatePerson(id, person);
            return updatedPerson is not null ? _mapper.Map<PersonDTO>(updatedPerson) : null;
        }

        public void DeletePerson(int id)
        {
            _personRepository.DeletePerson(id);
        }

        public PersonDTO? GetPerson(int id)
        {
            var person = _personRepository.GetPerson(id);
            return person is not null ? _mapper.Map<PersonDTO>(person) : null;
        }

        public IEnumerable<PersonDTO> GetAllPersons()
        {
            var persons = _personRepository.GetAllPersons();
            return _mapper.Map<IEnumerable<PersonDTO>>(persons);
        }
    }
}