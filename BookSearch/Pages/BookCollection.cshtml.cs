using BookSearch.Helpers;
using BookSearch.Services;
using BookSearchAPI;
using BookSearchAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.Pages
{
    public class BookCollectionModel : PageModel
    {
        private readonly ILogger<BookCollectionModel> _logger;
        private readonly BookDbContext _context;
        private readonly IRequestService _requestService;

        public List<Book> Books { get; set; }
        public List<BookRatingPair> BookScores { get; set; }
        public string CollectionName { get; set; }

        public BookCollectionModel(ILogger<BookCollectionModel> logger, BookDbContext context, IRequestService requestService)
        {
            _logger = logger;
            _context = context;
            _requestService = requestService;
        }

        public async Task<IActionResult> OnGetPersonal()
        {
            if (!HttpContext.User.TryGetId(out var userId))
            {
                return new RedirectToPageResult("LoginPage");
            }

            CollectionName = "Персональные рекомендации";

            var count = 3;
            var genres = _context.Users.First(u => u.Id == userId).LikedGenres;

            var recommendedBooks = await _requestService.SendRequest<List<BookRatingPair>>($"Book/Personal/{userId}");
            if (recommendedBooks == null || recommendedBooks.Count == 0 || recommendedBooks.All(b => b.Rating == 0))
            {
                return new RedirectToPageResult("BookCollection", "Top");
            }
            BookScores = recommendedBooks;

            recommendedBooks.Sort((b1, b2) => b2.Rating.CompareTo(b1.Rating));

            var recommendByGenres = recommendedBooks
                .Select(recommendation => _context.Books.Include(b => b.Genres).ThenInclude(g => g.Users)
                .First(b => b.Id == recommendation.BookId))
                .Where(recommendation => recommendation.Genres.Any(g => g.Users.Select(u => u.Id).Contains(userId))).ToList();

            if (recommendByGenres.Count() >= count)
            {
                Books = recommendByGenres.Take(count).ToList();
                return Page();
            }

            var recommendedNotByGenre = recommendedBooks
                .Select(recommendation => _context.Books.First(b => b.Id == recommendation.BookId))
                .Where(recommendation => !recommendByGenres.Contains(recommendation)).Take(count - recommendByGenres.Count()).ToList();

            Books = recommendByGenres.ToList();
            Books.AddRange(recommendedNotByGenre);

            return Page();
        }

        public async Task<IActionResult> OnGetTop()
        {
            CollectionName = "Лушчие книги";
            Books = await _requestService.SendRequest<List<Book>>("Book/Top");

            return Page();
        }

        public async Task<IActionResult> OnGetPopular()
        {
            CollectionName = "Попуряные книги";
            Books = await _requestService.SendRequest<List<Book>>("Book/Popular");

            return Page();
        }

        public void OnGetGenres()
        {
            CollectionName = "Подборки по жанрам";
        }

        public void OnGetRead()
        {
            CollectionName = "Прочитанные книги";
        }

        public void OnGetWishlist()
        {
            CollectionName = "Список желаний";
        }

        public async Task<IActionResult> OnPostSearch(string text)
        {
            CollectionName = "Результаты поиска";

            if (!HttpContext.User.TryGetId(out var userId))
            {
                return RedirectToPage("LoginPage");

            }

            Books = await _context.Books
                .Include(b => b.Authors)
                .Where(b => b.Name.Contains(text) || b.Authors.Any(a => a.Name.Contains(text)))
                .ToListAsync();

            return Page();
        }
    }
}