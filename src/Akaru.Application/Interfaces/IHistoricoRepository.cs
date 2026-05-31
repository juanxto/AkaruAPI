using Akaru.Domain.Entities;

namespace Akaru.Application.Interfaces;

public interface IHistoricoRepository
{
    Task<HistoricoRecomendacao?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<HistoricoRecomendacao>> ListarPorUsuarioAsync(int usuarioId, CancellationToken ct = default);
    Task<HistoricoRecomendacao> CriarAsync(HistoricoRecomendacao historico, CancellationToken ct = default);
}
