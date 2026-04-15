using System;
using System.Linq;
using System.Threading.Tasks;
using exercise_01_api.Data;
using exercise_01_api.Models;
using exercise_01_api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace exercise_01_tests
{
    public class LibraryServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            // Arrange
            var context = GetDbContext();
            var service = new LibraryService(context);

            // Act
            var books = await service.GetAllBooksAsync();

            // Assert
            Assert.Equal(9, books.Count());
        }

        [Fact]
        public async Task GetBookByIdAsync_ExistingId_ReturnsBook()
        {
            // Arrange
            var context = GetDbContext();
            var service = new LibraryService(context);

            // Act
            var book = await service.GetBookByIdAsync(1);

            // Assert
            Assert.NotNull(book);
            Assert.Equal("Harry Potter and the Philosopher's Stone", book.Title);
        }

        [Fact]
        public async Task GetAllAuthorsAsync_ReturnsAllAuthors()
        {
            // Arrange
            var context = GetDbContext();
            var service = new LibraryService(context);

            // Act
            var authors = await service.GetAllAuthorsAsync();

            // Assert
            Assert.Equal(2, authors.Count()); // Based on user instruction to seed only 2 authors
        }

        [Fact]
        public async Task AddAuthorAsync_SavesAuthor()
        {
            // Arrange
            var context = GetDbContext();
            var service = new LibraryService(context);
            var newAuthor = new Author { Name = "New Author", Biography = "Bio" };

            // Act
            var result = await service.AddAuthorAsync(newAuthor);

            // Assert
            Assert.NotEqual(0, result.Id);
            Assert.Equal(3, (await service.GetAllAuthorsAsync()).Count());
        }

        [Fact]
        public async Task AddBookAsync_SavesBook()
        {
            // Arrange
            var context = GetDbContext();
            var service = new LibraryService(context);
            var newBook = new Book { Title = "New Book", Genre = "Genre", AuthorId = 1, PublishedDate = DateTime.Now };

            // Act
            var result = await service.AddBookAsync(newBook);

            // Assert
            Assert.NotEqual(0, result.Id);
            Assert.Equal(10, (await service.GetAllBooksAsync()).Count());
        }

        [Fact]
        public async Task UpdateAuthorAsync_ExistingId_UpdatesFields()
        {
            // Arrange
            var context = GetDbContext();
            var service = new LibraryService(context);
            var updatedInfo = new Author { Name = "Updated Name", Biography = "Updated Bio" };

            // Act
            var result = await service.UpdateAuthorAsync(1, updatedInfo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.Name);
            Assert.Equal("Updated Bio", result.Biography);
        }
    }
}
