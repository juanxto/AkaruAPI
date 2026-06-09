using Akaru.Application.Interfaces;
using Akaru.Domain.Entities;
using Akaru.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Akaru.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AkaruDbContext _context;

    public UsuarioRepository(AkaruDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterPorFirebaseUidAsync(string firebaseUid, CancellationToken ct = default) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid, ct);

    public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<Usuario?> ObterPorIdAsync(int id, CancellationToken ct = default) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<Usuario> CriarAsync(Usuario usuario, CancellationToken ct = default)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync(ct);
        return usuario;
    }

    public async Task AtualizarAsync(Usuario usuario, CancellationToken ct = default)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync(ct);
    }
}
