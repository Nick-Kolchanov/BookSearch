using BookSearchAPI.Services;
using BookSearchAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.EntityFrameworkCore;

namespace BookSearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IModelInitService _modelInit;
        private readonly IBookService _bookService;
        private readonly PredictionEnginePool<BookRating, BookRatingPrediction> _model;

        public BookController(PredictionEnginePool<BookRating, BookRatingPrediction> model,
            BookDbContext context,
            IModelInitService modelInit,
            IBookService bookService)
        {
            _context = context;
            _modelInit = modelInit;
            _bookService = bookService;
            _model = model;
        }

        [HttpGet("QuickRecommend")]
        public Book GetQuickRecommend()
        {
            return _context.Books.FirstOrDefault(book => book.Id == 3);
        }

        [HttpGet("Top")]
        public async Task<List<Book>> GetTop()
        {
            return await _bookService.GetTopBooks();
        }

        [HttpGet("Popular")]
        public async Task<List<Book>> GetPopular()
        {
            return await _bookService.GetPopularBooks();
        }

        [HttpGet("Personal/{userId:int}")]
        public List<BookRatingPair> GetRecommendations(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return new List<BookRatingPair>();
            }

            _modelInit.InitModel(_context);

            var ratings = new List<BookRatingPair>();

            BookRatingPrediction prediction = null;
            foreach (var book in _context.Books.Include(b => b.UserBookRating))
            {
                if (book.UserBookRating.Any(ub => ub.UserId == userId))
                {
                    continue;
                }

                prediction = _model.Predict(new BookRating
                {
                    userId = userId,
                    bookId = book.Id
                });

                float normalizedscore = Sigmoid(prediction.Score);
                ratings.Add( new BookRatingPair { BookId = book.Id, Rating = normalizedscore });
            }

            return ratings;
        }

        public float Sigmoid(float x)
        {
            return (float)(100 / (1 + Math.Exp(-x)));
        }
    }
}