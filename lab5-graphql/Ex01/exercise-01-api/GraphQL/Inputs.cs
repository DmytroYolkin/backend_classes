using System;
using System.Collections.Generic;

namespace exercise_01_api.GraphQL
{
    public class AddAuthorInput
    {
        public required string Name { get; set; }
        public string? Biography { get; set; }
    }

    public class AddBookInput
    {
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }
    }

    public class UpdateAuthorInput
    {
        public required string Name { get; set; }
        public string? Biography { get; set; }
    }
}
