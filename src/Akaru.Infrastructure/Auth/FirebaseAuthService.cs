using Akaru.Application.Interfaces;
using Akaru.Domain.Exceptions;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Akaru.Infrastructure.Auth;

public class FirebaseAuthService : IFirebaseAuthService
{
    private readonly ILogger<FirebaseAuthService> _logger;

    public FirebaseAuthService(IConfiguration configuration, ILogger<FirebaseAuthService> logger)
    {
        _logger = logger;
        InicializarFirebase(configuration, logger);
    }

    public async Task<FirebaseUserInfo> ValidarTokenAsync(string idToken, CancellationToken ct = default)
    {
        if (FirebaseApp.DefaultInstance is null)
            throw new AcessoNegadoException("Firebase Admin SDK não configurado.");

        try
        {
            var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken, ct);
            var email = decoded.Claims.TryGetValue("email", out var emailValue)
                ? emailValue?.ToString()
                : null;
            var name = decoded.Claims.TryGetValue("name", out var nameValue)
                ? nameValue?.ToString()
                : null;

            return new FirebaseUserInfo(decoded.Uid, email, name);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao validar token Firebase.");
            throw new AcessoNegadoException("Token Firebase inválido ou expirado.");
        }
    }

    private static void InicializarFirebase(IConfiguration configuration, ILogger logger)
    {
        if (FirebaseApp.DefaultInstance is not null)
            return;

        var credentialsPath = configuration["Firebase:CredentialsPath"];
        var projectId = configuration["Firebase:ProjectId"];

        if (string.IsNullOrWhiteSpace(credentialsPath) || !File.Exists(credentialsPath))
        {
            logger.LogWarning(
                "Firebase credentials não encontradas em {Path}. Configure Firebase:CredentialsPath.",
                credentialsPath);
            return;
        }

        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(credentialsPath),
            ProjectId = projectId
        });
    }
}
