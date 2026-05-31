using Akaru.Application.Interfaces;
using Akaru.Domain.Entities;
using Akaru.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Akaru.Infrastructure.Repositories;

public class PlantioRepository : IPlantioRepository
{
    private readonly AkaruDbContext _context;

    public PlantioRepository(AkaruDbContext context)
    {
        _context = context;
    }

    public async Task<Plantio?> ObterPorIdAsync(int id, CancellationToken ct = default) =>
        await _context.Plantios
            .Include(p => p.PlantioCulturas)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IReadOnlyList<Plantio>> ListarPorUsuarioAsync(int usuarioId, CancellationToken ct = default) =>
        await _context.Plantios
            .Include(p => p.PlantioCulturas)
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.DataPlantio)
            .ToListAsync(ct);

    public async Task<Plantio> CriarAsync(Plantio plantio, CancellationToken ct = default)
    {
        _context.Plantios.Add(plantio);
        await _context.SaveChangesAsync(ct);
        return plantio;
    }

    public async Task AtualizarAsync(Plantio plantio, CancellationToken ct = default)
    {
        _context.Plantios.Update(plantio);
        await _context.SaveChangesAsync(ct);
    }

    public async Task RemoverAsync(Plantio plantio, CancellationToken ct = default)
    {
        _context.Plantios.Remove(plantio);
        await _context.SaveChangesAsync(ct);
    }
}
