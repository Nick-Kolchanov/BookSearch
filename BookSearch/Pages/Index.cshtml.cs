using BookSearch.Services;
using BookSearchAPI.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookSearch.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRequestService _requestService;

        public Book QuickRecommend;

        public IndexModel(ILogger<IndexModel> logger, IRequestService requestService)
        {
            _logger = logger;
            _requestService = requestService;
        }

        public async Task OnGet()
        {
            QuickRecommend = await _requestService.SendRequest<Book>("Book/QuickRecommend");
        }
    }
}