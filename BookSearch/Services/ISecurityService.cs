namespace BookSearch.Services
{
    public interface ISecurityService
    {
        public string HashPassword(char[] password);
    }
}
