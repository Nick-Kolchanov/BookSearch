using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    public interface IBookService
    {
        public List<Book> GetPopularBooks();
        public List<Book> GetRandomBooks();
    }
}
