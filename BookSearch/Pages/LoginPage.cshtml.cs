using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using BookSearchAPI;
using BookSearch.Services;

namespace BookSearch.Pages
{
    public class LoginPageModel : PageModel
    {
        private readonly BookDbContext _dbContext;
        private readonly ISecurityService _securityService;

        public LoginPageModel(BookDbContext dbContext, ISecurityService securityService)
        {
            _dbContext = dbContext;
            _securityService = securityService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost(string login, string password)
        {
            var passHash = _securityService.HashPassword(password.ToCharArray());

            var user = _dbContext.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == passHash);
            if (user == null)
            {
                ModelState.AddModelError("", "Такого пользователя нет");
                return Page();
            }

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Login));

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToPage("Index");
        }
    }
}
