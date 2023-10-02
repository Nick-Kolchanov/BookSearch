namespace BookSearch.Services
{
    public interface IRequestService
    {
        public Task<T?> SendRequest<T>(string url);
    }
}
