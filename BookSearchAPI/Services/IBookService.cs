using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    public interface IBookService
    {
        public Task<List<Book>> GetPopularBooks();
        public Task<List<Book>> GetTopBooks();
    }
}
