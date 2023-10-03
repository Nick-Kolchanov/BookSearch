using BookSearchAPI.Models;
using Microsoft.ML;

namespace BookSearchAPI.Services
{
    public interface IModelInitService
    {
        public PredictionEngine<BookRating, BookRatingPrediction> InitModel(BookDbContext dbContext);
    }
}
