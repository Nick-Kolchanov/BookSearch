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
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(_configuration["baseApiUrl"] + "/Book/3"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewData["Books"] = JsonConvert.DeserializeObject<List<BookRatingPair>>(apiResponse);
                }
            }
        }
    }
}