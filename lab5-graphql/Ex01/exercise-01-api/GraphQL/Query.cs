using System.Linq;
using System.Threading.Tasks;
using exercise_01_api.Models;
using exercise_01_api.Services;
using HotChocolate;
using HotChocolate.Data;

namespace exercise_01_api.GraphQL
{
    public class Query
    {
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<Book>> GetBooks([Service] ILibraryService libraryService)
            => await libraryService.GetAllBooksAsync();

        public async Task<Book?> GetBookById([Service] ILibraryService libraryService, int id)
            => await libraryService.GetBookByIdAsync(id);

        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<Author>> GetAuthors([Service] ILibraryService libraryService)
            => await libraryService.GetAllAuthorsAsync();
    }
}