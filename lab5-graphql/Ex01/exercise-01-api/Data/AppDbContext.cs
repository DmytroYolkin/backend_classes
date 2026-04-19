using Microsoft.EntityFrameworkCore;
using System;
using exercise_01_api.Models;

namespace exercise_01_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId);
                
            var author1 = new Author { Id = 1, Name = "J.K. Rowling", Biography = "British author, best known for the Harry Potter series." };
            var author2 = new Author { Id = 2, Name = "George Orwell", Biography = "English novelist and essayist, journalist and critic." };
            var author3 = new Author { Id = 3, Name = "Jane Austen", Biography = "English novelist known primarily for her six major novels." };
            var author4 = new Author { Id = 4, Name = "Mark Twain", Biography = "American writer, humorist, entrepreneur, publisher, and lecturer." };
            var author5 = new Author { Id = 5, Name = "Agatha Christie", Biography = "English writer known for her sixty-six detective novels and fourteen short story collections." };

            modelBuilder.Entity<Author>().HasData(author1, author2);

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Harry Potter and the Philosopher's Stone", Genre = "Fantasy", PublishedDate = new DateTime(1997, 6, 26), AuthorId = 1 },
                new Book { Id = 2, Title = "Harry Potter and the Chamber of Secrets", Genre = "Fantasy", PublishedDate = new DateTime(1998, 7, 2), AuthorId = 1 },
                new Book { Id = 3, Title = "1984", Genre = "Dystopian", PublishedDate = new DateTime(1949, 6, 8), AuthorId = 2 },
                new Book { Id = 4, Title = "Animal Farm", Genre = "Satire", PublishedDate = new DateTime(1945, 8, 17), AuthorId = 2 },
                new Book { Id = 5, Title = "Pride and Prejudice", Genre = "Romance", PublishedDate = new DateTime(1813, 1, 28), AuthorId = 3 },
                new Book { Id = 6, Title = "Sense and Sensibility", Genre = "Romance", PublishedDate = new DateTime(1811, 10, 30), AuthorId = 3 },
                new Book { Id = 7, Title = "Adventures of Huckleberry Finn", Genre = "Adventure", PublishedDate = new DateTime(1884, 12, 10), AuthorId = 4 },
                new Book { Id = 8, Title = "The Adventures of Tom Sawyer", Genre = "Adventure", PublishedDate = new DateTime(1876, 6, 1), AuthorId = 4 },
                new Book { Id = 9, Title = "Murder on the Orient Express", Genre = "Mystery", PublishedDate = new DateTime(1934, 1, 1), AuthorId = 5 }
            );
        }
    }
}