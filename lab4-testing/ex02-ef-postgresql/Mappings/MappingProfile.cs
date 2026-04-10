using AutoMapper;
using Howest.lab2.ex02_ef_postgresql.Models;
using Howest.lab2.ex02_ef_postgresql.DTOs;

namespace Howest.lab2.ex02_ef_postgresql.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();
        }
    }
}
