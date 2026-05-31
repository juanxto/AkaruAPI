using Akaru.Domain.Entities;

namespace Akaru.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorFirebaseUidAsync(string firebaseUid, CancellationToken ct = default);
    Task<Usuario?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<Usuario> CriarAsync(Usuario usuario, CancellationToken ct = default);
    Task AtualizarAsync(Usuario usuario, CancellationToken ct = default);
}
