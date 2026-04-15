using System.Threading.Tasks;
using exercise_01_api.Models;
using exercise_01_api.Services;
using HotChocolate;

namespace exercise_01_api.GraphQL
{
    public class Mutation
    {
        public async Task<Author> AddAuthor([Service] ILibraryService libraryService, AddAuthorInput input)
            => await libraryService.AddAuthorAsync(new Author { Name = input.Name, Biography = input.Biography });

        public async Task<Book> AddBook([Service] ILibraryService libraryService, AddBookInput input)
            => await libraryService.AddBookAsync(new Book { Title = input.Title, Genre = input.Genre, PublishedDate = input.PublishedDate, AuthorId = input.AuthorId });

        public async Task<Author?> UpdateAuthor([Service] ILibraryService libraryService, int id, UpdateAuthorInput input)
            => await libraryService.UpdateAuthorAsync(id, new Author { Name = input.Name, Biography = input.Biography });
    }
}