using System.Security.Claims;

namespace Akaru.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string ObterFirebaseUid(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? user.FindFirstValue("user_id")
        ?? throw new UnauthorizedAccessException("Token sem identificador de usuário.");
}
