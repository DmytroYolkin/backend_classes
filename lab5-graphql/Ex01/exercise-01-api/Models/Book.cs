using System;

namespace exercise_01_api.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public DateTime PublishedDate { get; set; }
        
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
