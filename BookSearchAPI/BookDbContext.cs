using BookSearchAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSearchAPI
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Name = "1",
                    Description = "William",
                    Category = "Shakespeare"
                },
                new Book
                {
                    Id = 2,
                    Name = "2",
                    Description = "Gregory",
                    Category = "Smith"
                }, new Book
                {
                    Id = 3,
                    Name = "3",
                    Description = "William",
                    Category = "Shakespeare"
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Nick",
                    Login = "admin",
                    PasswordHash = "a"
                },
                new User
                {
                    Id = 2,
                    Name = "Alex",
                    Login = "Alex",
                    PasswordHash = "a"
                }, new User
                {
                    Id = 3,
                    Name = "Mary",
                    Login = "Mary",
                    PasswordHash = "a"
                }
            );

            modelBuilder.Entity<UserBookRating>().HasData(
                new UserBookRating
                {
                    Id = 1,
                    UserId = 1,
                    BookId = 1,
                    Rating = 5
                },
                new UserBookRating
                {
                    Id = 2,
                    UserId = 1,
                    BookId = 2,
                    Rating = 5
                }, new UserBookRating
                {
                    Id = 3,
                    UserId = 1,
                    BookId = 3,
                    Rating = 1
                }, new UserBookRating
                {
                    Id = 4,
                    UserId = 2,
                    BookId = 1,
                    Rating = 1
                }, new UserBookRating
                {
                    Id = 5,
                    UserId = 2,
                    BookId = 2,
                    Rating = 1
                }, new UserBookRating
                {
                    Id = 6,
                    UserId = 2,
                    BookId = 3,
                    Rating = 5
                },
                new UserBookRating
                {
                    Id = 7,
                    UserId = 3,
                    BookId = 1,
                    Rating = 5
                }
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBookRating> Ratings { get; set; }
    }
}
