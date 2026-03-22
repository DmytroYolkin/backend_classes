using Howest.lab2.ex02_ef_postgresql.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.lab2.ex02_ef_postgresql.Data
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
        }
    }
}
