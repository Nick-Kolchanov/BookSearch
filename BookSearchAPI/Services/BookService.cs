using BookSearchAPI.Controllers;
using BookSearchAPI.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Book>> GetPopularBooks()
        {
            return await _dbContext.Books.Where(book => book.Id == 1 || book.Id == 2).ToListAsync();
        }

        public async Task<List<Book>> GetTopBooks()
        {
            return await _dbContext.Books.Where(book => book.Id == 3).ToListAsync();
        }
    }
}
