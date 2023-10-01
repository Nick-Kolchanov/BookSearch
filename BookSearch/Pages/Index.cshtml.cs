using BookSearchAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace BookSearch.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task OnGet()
        {
            ViewData["Books"] = await SendRequest<List<BookRatingPair>>(_configuration["baseApiUrl"] + "/Book/3");
            ViewData["QuickRecommend"] = new Book { Id = 3, Name = "Унесенные ветром", Description = ""};
            /*using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(_configuration["baseApiUrl"] + "/Book/3"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewData["Books"] = JsonConvert.DeserializeObject<List<BookRatingPair>>(apiResponse);
                }
            }*/
        }

        public async Task<T?> SendRequest<T>(string url)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
            }
        }
    }
}