using Microsoft.ML.Data;

namespace BookSearchAPI.Models
{
    public class BookRating
    {
        [LoadColumn(0)]
        public float userId;
        [LoadColumn(1)]
        public float bookId;
        [LoadColumn(2)]
        public float Label;
    }
}
