using Microsoft.ML;
using Microsoft.ML.Trainers;
using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    public class ModelInitService : IModelInitService
    {
        private readonly MLContext _mlContext;
        private readonly BookDbContext _dbContext;
        private readonly ILogger<ModelInitService> _logger;
        private readonly IConfiguration _configurator;

        public ModelInitService(BookDbContext dbContext, ILogger<ModelInitService> logger, IConfiguration configurator)
        {
            _mlContext = new MLContext();
            _dbContext = dbContext;
            _logger = logger;
            _configurator = configurator;
        }

        public void InitModel()
        {
            if (!int.TryParse(_configurator["IterationsCount"], out var iterCount) ||
                !int.TryParse(_configurator["ApproximatonRank"], out var approxRank))
            {
                _logger.LogError("Error while reading int from Configuration");
                return;
            }

            var data = LoadData();
            var model = BuildAndTrainModel(data, iterCount, approxRank);

            //var score = UseModelForSinglePrediction(model);

            SaveModel(data.Schema, model);
        }

        IDataView LoadData()
        {
            //calling .ToArray() for query execution
            var data = _mlContext.Data.LoadFromEnumerable(
                _dbContext.Ratings
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

        void UseModelForSinglePrediction(ITransformer model)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<BookRating, BookRatingPrediction>(model);

            var testInput = new BookRating { userId = 3, bookId = 3 };

            var movieRatingPrediction = predictionEngine.Predict(testInput);

            _logger.LogInformation(movieRatingPrediction.Score.ToString());
        }

        void SaveModel(DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            var modelPath = Path.Combine(Environment.CurrentDirectory, _configurator["MLModelPath"]);

            _mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
        }
    }
}
