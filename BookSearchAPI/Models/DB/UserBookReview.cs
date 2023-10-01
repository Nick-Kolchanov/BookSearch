namespace BookSearchAPI.Models
{
    public class UserBookReview
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string Review { get; set; }
    }
}
