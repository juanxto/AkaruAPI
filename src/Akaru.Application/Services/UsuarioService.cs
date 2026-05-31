using Akaru.Application.DTOs;
using Akaru.Application.Interfaces;
using Akaru.Domain.Entities;
using Akaru.Domain.Exceptions;

namespace Akaru.Application.Services;

public class UsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<UsuarioResponseDto> SincronizarAsync(FirebaseUserInfo firebaseUser, CancellationToken ct = default)
    {
        var existente = await _repository.ObterPorFirebaseUidAsync(firebaseUser.Uid, ct);

        if (existente is not null)
            return Mapear(existente);

        var usuario = new Usuario
        {
            FirebaseUid = firebaseUser.Uid,
            Email = firebaseUser.Email ?? $"{firebaseUser.Uid}@akaru.local",
            Nome = firebaseUser.Name ?? firebaseUser.Email?.Split('@')[0] ?? "Agricultor",
            DataCadastro = DateTime.UtcNow
        };

        var criado = await _repository.CriarAsync(usuario, ct);
        return Mapear(criado);
    }

    public async Task<UsuarioResponseDto> ObterPerfilAsync(int usuarioId, CancellationToken ct = default)
    {
        var usuario = await _repository.ObterPorIdAsync(usuarioId, ct)
            ?? throw new NotFoundException("Usuário não encontrado.");

        return Mapear(usuario);
    }

    public async Task<UsuarioResponseDto> AtualizarPerfilAsync(int usuarioId, AtualizarUsuarioDto dto, CancellationToken ct = default)
    {
        var usuario = await _repository.ObterPorIdAsync(usuarioId, ct)
            ?? throw new NotFoundException("Usuário não encontrado.");

        if (!string.IsNullOrWhiteSpace(dto.Nome))
            usuario.Nome = dto.Nome;

        if (dto.Latitude.HasValue)
            usuario.Latitude = dto.Latitude;

        if (dto.Longitude.HasValue)
            usuario.Longitude = dto.Longitude;

        if (dto.Cidade is not null)
            usuario.Cidade = dto.Cidade;

        if (dto.Estado is not null)
            usuario.Estado = dto.Estado;

        await _repository.AtualizarAsync(usuario, ct);
        return Mapear(usuario);
    }

    public async Task<int> ObterIdPorFirebaseUidAsync(string firebaseUid, CancellationToken ct = default)
    {
        var usuario = await _repository.ObterPorFirebaseUidAsync(firebaseUid, ct)
            ?? throw new NotFoundException("Usuário não sincronizado. Chame POST /api/usuarios/sync primeiro.");

        return usuario.Id;
    }

    private static UsuarioResponseDto Mapear(Usuario usuario) =>
        new(usuario.Id, usuario.Nome, usuario.Email, usuario.Latitude, usuario.Longitude,
            usuario.Cidade, usuario.Estado, usuario.DataCadastro);
}
