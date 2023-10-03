using BookSearch.Helpers;
using BookSearchAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly ILogger<SettingsModel> _logger;
        private readonly BookDbContext _context;

        public List<int> SelectedGenres;

        public SettingsModel(ILogger<SettingsModel> logger, BookDbContext context)
        {
            _logger = logger;
            _context = context;
            SelectedGenres = new List<int>();
        }

        public async Task<IActionResult> OnGet()
        {
            if (!HttpContext.User.TryGetId(out var userId))
            {
                return new RedirectToPageResult("LoginPage");
            }

            var user = _context.Users.Include(u => u.LikedGenres).First(u => u.Id == userId);

            foreach (var genre in user.LikedGenres)
            {
                SelectedGenres.Add(genre.Id);
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(int[] selectedGenres)
        {
            if (!HttpContext.User.TryGetId(out var userId))
            {
                return new RedirectToPageResult("LoginPage");
            }

            var user = _context.Users.Include(u => u.LikedGenres).First(u => u.Id == userId);
            user.LikedGenres.Clear();
            _context.SaveChanges();

            for (int i = 0; i < selectedGenres.Length; i++)
            {
                var genre = _context.Genres.Include(g => g.Users).Single(g => g.Id == selectedGenres[i]);
                user.LikedGenres.Add(genre);
            }
            _context.SaveChanges();

            return RedirectToPage("Settings");
        }
    }
}