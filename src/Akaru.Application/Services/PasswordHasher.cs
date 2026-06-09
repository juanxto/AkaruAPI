using System.Security.Cryptography;
using System.Text;

namespace Akaru.Application.Services;

public static class PasswordHasher
{
    public static string Hash(string senha)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(senha),
            salt,
            100_000,
            HashAlgorithmName.SHA256,
            32);

        return $"pbkdf2.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verificar(string senha, string? armazenada)
    {
        if (string.IsNullOrWhiteSpace(armazenada))
            return false;

        if (!armazenada.StartsWith("pbkdf2.", StringComparison.Ordinal))
            return armazenada == senha;

        var partes = armazenada.Split('.', 3);
        if (partes.Length != 3)
            return false;

        var salt = Convert.FromBase64String(partes[1]);
        var hashEsperado = Convert.FromBase64String(partes[2]);
        var hashAtual = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(senha),
            salt,
            100_000,
            HashAlgorithmName.SHA256,
            32);

        return CryptographicOperations.FixedTimeEquals(hashAtual, hashEsperado);
    }
}
