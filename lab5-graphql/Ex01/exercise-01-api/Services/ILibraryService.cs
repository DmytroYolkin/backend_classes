using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exercise_01_api.Models;

namespace exercise_01_api.Services
{
    public interface ILibraryService
    {
        Task<IQueryable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<IQueryable<Author>> GetAllAuthorsAsync();
        
        Task<Author> AddAuthorAsync(Author author);
        Task<Book> AddBookAsync(Book book);
        Task<Author?> UpdateAuthorAsync(int id, Author updatedAuthor);
    }
}