namespace Akaru.Application.Interfaces;

public record FirebaseUserInfo(string Uid, string? Email, string? Name);

public interface IFirebaseAuthService
{
    Task<FirebaseUserInfo> ValidarTokenAsync(string idToken, CancellationToken ct = default);
}
