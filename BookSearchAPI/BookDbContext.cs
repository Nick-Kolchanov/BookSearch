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
            modelBuilder.Entity<AgeRestriction>().HasData(
                new AgeRestriction
                {
                    Id = 1,
                    Name = "0+",
                    MinAge = 0,
                },
                new AgeRestriction
                {
                    Id = 2,
                    Name = "6+",
                    MinAge = 6,
                },
                new AgeRestriction
                {
                    Id = 3,
                    Name = "12+",
                    MinAge = 12,
                },
                new AgeRestriction
                {
                    Id = 4,
                    Name = "16+",
                    MinAge = 16,
                },
                new AgeRestriction
                {
                    Id = 5,
                    Name = "18+",
                    MinAge = 18,
                }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    Name = "Маргарет Митчелл"
                },
                new Author
                {
                    Id = 2,
                    Name = "Терри Пратчетт"
                }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre
                {
                    Id = 1,
                    Name = "Фентези"
                },
                new Genre
                {
                    Id = 2,
                    Name = "Детектив"
                },
                new Genre
                {
                    Id = 3,
                    Name = "Исторический роман"
                },
                new Genre
                {
                    Id = 4,
                    Name = "Романтика"
                }
            );

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookAuthors",
                    r => r.HasOne<Author>().WithMany().HasForeignKey("AuthorId"),
                    l => l.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                    je =>
                    {
                        je.HasKey("BookId", "AuthorId");
                        je.HasData(
                            new { BookId = 1, AuthorId = 2 },
                            new { BookId = 2, AuthorId = 2 },
                            new { BookId = 3, AuthorId = 1 });
                    });

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Genres)
                .WithMany(a => a.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookGenres",
                    r => r.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                    l => l.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                    je =>
                    {
                        je.HasKey("BookId", "GenreId");
                        je.HasData(
                            new { BookId = 1, GenreId = 1 },
                            new { BookId = 2, GenreId = 1 },
                            new { BookId = 3, GenreId = 3 },
                            new { BookId = 3, GenreId = 4 });
                    });

            modelBuilder.Entity<Book>()
                .HasOne(b => b.BookPicture)
                .WithOne(p => p.Book)
                .HasForeignKey<BookPicture>(p => p.Id);

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Name = "Стража! Стража!",
                    Description = "Фантастические романы.Значит, так. Анк-Морпорк - это вам не какая-нибудь овцепикская глушь! Тут все сплошь цивилизация. Здесь продают сосиски в тесте, грабят и убивают (причем иногда без лицензии!), шутят и баламутят, химичат и магичат. Ясное дело, бывают и непорядки. Вот на этот случай тут и есть Стража.",
                    AgeRestrictionId = 4,
                    PublicationYear = 1989,
                }, new Book
                {
                    Id = 2,
                    Name = "К оружию! К оружию!",
                    Description = "В Плоском мире грядут серьезные перемены. Сэм Ваймс, командующий Городской стражей, собирается жениться на леди Сибилле Овнец и отойти от дел. В городе и Гильдиях творится хаос. Некое смертельное, загадочное оружие совершает несколько жестоких и бессмысленных убийств.",
                    AgeRestrictionId = 5,
                    PublicationYear = 1993,
                },
                new Book
                {
                    Id = 3,
                    Name = "Унесенные ветром",
                    Description = "Согласно легенде, создание романа \"Унесенные ветром\" началось с того, как Маргарет Митчелл написала главную фразу последней главы: \"Ни одного из любимых ею мужчин Скарлетт так и не смогла понять и вот - потеряла обоих\". Последующая работа над произведением продолжалась около десяти лет и потребовала от писательницы огромной самоотдачи и напряженного труда.",
                    AgeRestrictionId = 3,
                    PublicationYear = 1936,
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Никита",
                    Login = "Nick",
                    PasswordHash = "e29201410cd1f8f4a3ec4e596584294f24da70bbed4b66026c3b41d2ac6b3865",
                    Phone = "+79990005555"
                },
                new User
                {
                    Id = 2,
                    Name = "Алексей",
                    Login = "Alex",
                    PasswordHash = "e29201410cd1f8f4a3ec4e596584294f24da70bbed4b66026c3b41d2ac6b3865"
                }, new User
                {
                    Id = 3,
                    Name = "Мария",
                    Login = "Mary",
                    PasswordHash = "e29201410cd1f8f4a3ec4e596584294f24da70bbed4b66026c3b41d2ac6b3865"
                }
            );

            modelBuilder.Entity<UserBookRating>().HasData(
                new UserBookRating
                {
                    Id = 1,
                    UserId = 1,
                    BookId = 1,
                    Rating = 10
                },
                new UserBookRating
                {
                    Id = 2,
                    UserId = 1,
                    BookId = 2,
                    Rating = 10
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
                    Rating = 10
                }
            );

            modelBuilder.Entity<UserBookReview>().HasData(
                new UserBookReview
                {
                    Id = 1,
                    UserId = 1,
                    BookId = 1,
                    Review = "Классный роман!"
                }
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBookRating> Ratings { get; set; }
        public DbSet<UserBookReview> Reviews { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AgeRestriction> AgeRestrictions { get; set; }
    }
}
