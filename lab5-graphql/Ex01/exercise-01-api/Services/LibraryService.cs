using System.Linq;
using System.Threading.Tasks;
using exercise_01_api.Data;
using exercise_01_api.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise_01_api.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly AppDbContext _context;

        public LibraryService(AppDbContext context)
        {
            _context = context;
        }

        public Task<IQueryable<Book>> GetAllBooksAsync()
        {
            return Task.FromResult(_context.Books.AsQueryable());
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public Task<IQueryable<Author>> GetAllAuthorsAsync()
        {
            return Task.FromResult(_context.Authors.AsQueryable());
        }

        public async Task<Author> AddAuthorAsync(Author author)
        {
            author.Id = _context.Authors.Any() ? _context.Authors.Max(a => a.Id) + 1 : 1;
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            book.Id = _context.Books.Any() ? _context.Books.Max(b => b.Id) + 1 : 1;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Author?> UpdateAuthorAsync(int id, Author updatedAuthor)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return null;

            author.Name = updatedAuthor.Name;
            author.Biography = updatedAuthor.Biography;

            await _context.SaveChangesAsync();
            return author;
        }
    }
}