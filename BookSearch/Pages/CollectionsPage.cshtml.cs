using BookSearch.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookSearch.Pages
{
    public class CollectionsPageModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!HttpContext.User.TryGetId(out var _))
            {
                return new RedirectToPageResult("LoginPage");
            }

            return Page();
        }
    }
}