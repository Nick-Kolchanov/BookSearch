using BookSearchAPI.Controllers;
using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    public class BookService : IBookService
    {
        private readonly BookDbContext _dbContext;
        private readonly ILogger<BookController> _logger;

        public BookService(BookDbContext dbContext, ILogger<BookController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public List<Book> GetPopularBooks()
        {
            return _dbContext.Books.ToList();
        }

        public List<Book> GetRandomBooks()
        {
            throw new NotImplementedException();
        }
    }
}
