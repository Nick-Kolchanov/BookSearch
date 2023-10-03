using System.Security.Claims;

namespace BookSearch.Helpers
{
    public static class UserExtensions
    {
        public static bool TryGetId(this ClaimsPrincipal user, out int userId)
        {
            userId = 0;

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (user.Identity.IsAuthenticated && userIdClaim != null && int.TryParse(userIdClaim.Value, out userId))
            {
                return true;
            }

            return false;
        }
    }
}
