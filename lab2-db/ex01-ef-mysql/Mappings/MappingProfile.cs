using AutoMapper;
using Howest.lab2.ex01_ef_mysql.Models;
using Howest.lab2.ex01_ef_mysql.DTOs;

namespace Howest.lab2.ex01_ef_mysql.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();
        }
    }
}