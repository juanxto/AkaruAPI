using System.Security.Claims;

namespace Akaru.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int ObterUsuarioId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub");

        if (int.TryParse(sub, out var usuarioId))
            return usuarioId;

        throw new UnauthorizedAccessException("Token JWT inválido ou sem identificador do usuário.");
    }
}
