using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using BookSearchAPI;
using BookSearch.Services;

namespace BookSearch.Pages
{
    public class RegisterPageModel : PageModel
    {
        private readonly BookDbContext _dbContext;
        private readonly ISecurityService _securityService;

        public RegisterPageModel(BookDbContext dbContext, ISecurityService securityService)
        {
            _dbContext = dbContext;
            _securityService = securityService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost(string name, string login, string password)
        {
            var passHash = _securityService.HashPassword(password.ToCharArray());
            _dbContext.Users.Add(new BookSearchAPI.Models.User { Name = name, Login = login, PasswordHash = passHash });
            await _dbContext.SaveChangesAsync();

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, login));

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToPage("Index");
        }
    }
}
