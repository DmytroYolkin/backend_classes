using Howest.lab2.ex01_ef_mysql.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.lab2.ex01_ef_mysql.Data
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
        }
    }
}