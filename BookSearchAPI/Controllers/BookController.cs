using BookSearchAPI.Services;
using BookSearchAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using System.Collections.Generic;

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

        [HttpGet("{userId:int}")]
        public async IAsyncEnumerable<BookRatingPair> GetRecommendatons(int userId)
        {
            //var activeprofile = _profileService.GetProfileByID(id);

            //_modelInit.InitModel();

            var ratings = new List<BookRatingPair>();

            BookRatingPrediction prediction = null;
            foreach (var book in _bookService.GetPopularBooks())
            {
                prediction = _model.Predict(new BookRating
                {
                    userId = userId,
                    bookId = book.Id
                });

                float normalizedscore = Sigmoid(prediction.Score);
                yield return new BookRatingPair { BookId = book.Id, Rating = normalizedscore };
            }
        }

        [HttpPost]
        public void SetScore(int userId, int bookId)
        {
            throw new NotImplementedException();
        }

        public float Sigmoid(float x)
        {
            return (float)(100 / (1 + Math.Exp(-x)));
        }
    }
}