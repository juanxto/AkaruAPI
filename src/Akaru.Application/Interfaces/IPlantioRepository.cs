using Akaru.Domain.Entities;

namespace Akaru.Application.Interfaces;

public interface IPlantioRepository
{
    Task<Plantio?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Plantio>> ListarPorUsuarioAsync(int usuarioId, CancellationToken ct = default);
    Task<Plantio> CriarAsync(Plantio plantio, CancellationToken ct = default);
    Task AtualizarAsync(Plantio plantio, CancellationToken ct = default);
    Task RemoverAsync(Plantio plantio, CancellationToken ct = default);
}
