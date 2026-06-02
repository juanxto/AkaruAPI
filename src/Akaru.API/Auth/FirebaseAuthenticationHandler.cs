using System.Security.Claims;
using System.Text.Encodings.Web;
using Akaru.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Akaru.API.Auth;

public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IFirebaseAuthService _firebaseAuth;
    private readonly IConfiguration _configuration;

    public FirebaseAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IFirebaseAuthService firebaseAuth,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _firebaseAuth = firebaseAuth;
        _configuration = configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
            return AuthenticateResult.NoResult();

        var header = authHeader.ToString();
        if (!header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.Fail("Header Authorization inválido.");

        var token = header["Bearer ".Length..].Trim();
        if (string.IsNullOrWhiteSpace(token))
            return AuthenticateResult.Fail("Token não informado.");

        try
        {
            FirebaseUserInfo userInfo;

            if (_configuration.GetValue<bool>("Firebase:UseMockAuth"))
            {
                userInfo = new FirebaseUserInfo(
                    $"mock-{token.GetHashCode():X}",
                    $"{token}@dev.akaru.local",
                    "Dev User");
            }
            else
            {
                userInfo = await _firebaseAuth.ValidarTokenAsync(token, Context.RequestAborted);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userInfo.Uid),
                new(ClaimTypes.Email, userInfo.Email ?? string.Empty),
                new(ClaimTypes.Name, userInfo.Name ?? string.Empty)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Autenticação Firebase falhou.");
            return AuthenticateResult.Fail("Token inválido.");
        }
    }
}
