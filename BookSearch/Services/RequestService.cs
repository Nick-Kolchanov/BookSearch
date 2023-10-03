using Newtonsoft.Json;

namespace BookSearch.Services
{
    public class RequestService : IRequestService
    {
        private readonly IConfiguration _configuration;

        public RequestService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<T?> SendRequest<T>(string url)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(_configuration["baseApiUrl"] + url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        return default(T?);
                    }

                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
            }
        }
    }
}
