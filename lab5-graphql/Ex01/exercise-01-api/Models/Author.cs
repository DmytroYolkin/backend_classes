using System;
using System.Collections.Generic;

namespace exercise_01_api.Models
{
    public class Author
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Biography { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
