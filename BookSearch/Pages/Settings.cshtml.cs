using BookSearch.Helpers;
using BookSearch.Services;
using BookSearchAPI;
using BookSearchAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookSearch.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly ILogger<SettingsModel> _logger;
        private readonly BookDbContext _context;

        public Book QuickRecommend;

        public SettingsModel(ILogger<SettingsModel> logger, BookDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnPost(int[] genres)
        {
            if (!HttpContext.User.TryGetId(out var userId))
            {
                return new RedirectToPageResult("LoginPage");
            }

            var user = _context.Users.First(u => u.Id == userId);
            for (int i = 0; i < genres.Length; i++)
            {
                user.LikedGenres.Add(new Genre { Id = genres[i] });
            }
            _context.SaveChanges();

            return Page();
        }
    }
}