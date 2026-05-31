using Akaru.Application.Interfaces;
using Akaru.Domain.Entities;
using Akaru.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Akaru.Infrastructure.Repositories;

public class HistoricoRepository : IHistoricoRepository
{
    private readonly AkaruDbContext _context;

    public HistoricoRepository(AkaruDbContext context)
    {
        _context = context;
    }

    public async Task<HistoricoRecomendacao?> ObterPorIdAsync(int id, CancellationToken ct = default) =>
        await _context.Historicos.FirstOrDefaultAsync(h => h.Id == id, ct);

    public async Task<IReadOnlyList<HistoricoRecomendacao>> ListarPorUsuarioAsync(int usuarioId, CancellationToken ct = default) =>
        await _context.Historicos
            .Where(h => h.UsuarioId == usuarioId)
            .OrderByDescending(h => h.DataGeracao)
            .ToListAsync(ct);

    public async Task<HistoricoRecomendacao> CriarAsync(HistoricoRecomendacao historico, CancellationToken ct = default)
    {
        _context.Historicos.Add(historico);
        await _context.SaveChangesAsync(ct);
        return historico;
    }
}
