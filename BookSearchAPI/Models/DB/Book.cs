namespace BookSearchAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BookPictureId { get; set; }
        public BookPicture? BookPicture { get; set; }
        public List<Author> Authors { get; set; } = new();
        public List<Genre> Genres { get; set; } = new();
        public int AgeRestrictionId { get; set; }
        public AgeRestriction? AgeRestriction { get; set; }
        public List<UserBookRating> UserBookRating { get; set; } = new();
        public List<UserBookReview> UserBookReview { get; set; } = new();
        public int PublicationYear { get; set; }
    }
}
