namespace BookSearchAPI.Models
{
    public class BookPicture
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public byte[] Picture { get; set; }
    }
}
