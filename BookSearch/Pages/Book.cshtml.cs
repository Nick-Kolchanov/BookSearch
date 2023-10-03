using BookSearch.Helpers;
using BookSearchAPI;
using BookSearchAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.Pages
{
    public class BookModel : PageModel
    {
        private readonly ILogger<BookModel> _logger;
        private readonly BookDbContext _context;

        public Book Book { get; set; }
        public string BookRating { get; set; }
        public double? UserRating { get; set; }
        public string? UserReview { get; set; }

        public BookModel(ILogger<BookModel> logger, BookDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var book = _context.Books.Include(b => b.UserBookRating).Include(b => b.UserBookReview).FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return new NotFoundResult();
            }

            Book = book;
            BookRating = book.UserBookRating.Average(r => r.Rating).ToString("0.00");

            if (HttpContext.User.TryGetId(out var userId))
            {
                var userBookReview = _context.Reviews.FirstOrDefault(r => r.UserId == userId && r.BookId == id);
                if (userBookReview != null)
                {
                    UserReview = userBookReview.Review;
                }

                var userBookRating = _context.Ratings.FirstOrDefault(r => r.UserId == userId && r.BookId == id);
                if (userBookRating != null)
                {
                    UserRating = userBookRating.Rating;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostReview(string rating, string review, int bookId)
        {
            if (!int.TryParse(rating, out var intRating))
            {
                ModelState.AddModelError("", "Неверное значение рейтинга");
                return Page();
            }

            if (!HttpContext.User.TryGetId(out var userId))
            {
                return RedirectToPage("LoginPage");
                
            }

            _context.Ratings.Add(new UserBookRating { Rating = intRating, BookId = bookId, UserId = userId });

            if (review != string.Empty && review != null)
            {
                _context.Reviews.Add(new UserBookReview { Review = review, BookId = bookId, UserId = userId });
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Book", new { id = bookId });
        }

        public async Task<IActionResult> OnPostReviewDelete(int bookId)
        {
            if (!HttpContext.User.TryGetId(out var userId))
            {
                return RedirectToPage("LoginPage");

            }
            var userBookReview = _context.Reviews.FirstOrDefault(r => r.UserId == userId && r.BookId == bookId);
            if (userBookReview != null)
            {
                _context.Remove(userBookReview);
            }

            var userBookRating = _context.Ratings.FirstOrDefault(r => r.UserId == userId && r.BookId == bookId);
            if (userBookRating != null)
            {
                _context.Remove(userBookRating);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Book", new { id = bookId });
        }
    }
}