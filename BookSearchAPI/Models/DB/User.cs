namespace BookSearchAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public List<UserBookRating> Ratings { get; set; } = new();
        public List<UserBookReview> Reviews{ get; set; } = new();
        public List<Genre> LikedGenres { get; set; } = new();
    }
}
