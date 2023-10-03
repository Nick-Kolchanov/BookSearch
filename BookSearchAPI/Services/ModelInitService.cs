using Microsoft.ML;
using Microsoft.ML.Trainers;
using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    public class ModelInitService : IModelInitService
    {
        private readonly MLContext _mlContext;
        private readonly ILogger<ModelInitService> _logger;
        private readonly IConfiguration _configurator;

        private DateTime _lastModelInit;

        public ModelInitService(ILogger<ModelInitService> logger, IConfiguration configurator)
        {
            _mlContext = new MLContext();
            _logger = logger;
            _configurator = configurator;
        }

        public PredictionEngine<BookRating, BookRatingPrediction> InitModel(BookDbContext dbContext)
        {
            if (_lastModelInit.AddSeconds(int.Parse(_configurator["ModelUpdatingCooldown"])) > DateTime.Now)
            {
                // commented out for testing
                //return;
            }

            if (!int.TryParse(_configurator["IterationsCount"], out var iterCount) ||
                !int.TryParse(_configurator["ApproximatonRank"], out var approxRank))
            {
                _logger.LogError("Error while reading int from Configuration");
                return null;
            }

            _lastModelInit = DateTime.Now;

            var data = LoadData(dbContext);
            var model = BuildAndTrainModel(data, iterCount, approxRank);

            SaveModel(data.Schema, model);

            return _mlContext.Model.CreatePredictionEngine<BookRating, BookRatingPrediction>(model);
        }

        IDataView LoadData(BookDbContext dbContext)
        {
            //calling .ToArray() for query execution
            var data = _mlContext.Data.LoadFromEnumerable(
                dbContext.Ratings
                .Select(rating => new BookRating { userId = rating.UserId, bookId = rating.BookId, Label = rating.Rating })
                .ToArray());

            return data;
        }

        ITransformer BuildAndTrainModel(IDataView trainingDataView, int iterationsCount, int appproxRank)
        {
            var estimator = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: nameof(BookRating.userId))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "bookIdEncoded", inputColumnName: nameof(BookRating.bookId)));

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "bookIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = iterationsCount,
                ApproximationRank = appproxRank
            };

            var trainerEstimator = estimator.Append(_mlContext.Recommendation().Trainers.MatrixFactorization(options));

            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
        }

        void UseModelForPrediction(ITransformer model, BookDbContext context)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<BookRating, BookRatingPrediction>(model);

            var testInput1 = new BookRating { userId = 3, bookId = 1 };
            var testInput2 = new BookRating { userId = 3, bookId = 2 };
            var testInput3 = new BookRating { userId = 3, bookId = 3 };

            _logger.LogInformation(predictionEngine.Predict(testInput1).Score.ToString());
            _logger.LogInformation(predictionEngine.Predict(testInput2).Score.ToString());
            _logger.LogInformation(predictionEngine.Predict(testInput3).Score.ToString());
        }

        void SaveModel(DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            //var modelPath = Path.Combine(Environment.CurrentDirectory, _configurator["MLModelPath"]);

            using (var stream = new FileStream(_configurator["MLModelPath"], FileMode.Create, FileAccess.Write, FileShare.None))
            {
                _mlContext.Model.Save(model, trainingDataViewSchema, stream);
            }
                
            _logger.LogInformation("Saved");
        }
    }
}
