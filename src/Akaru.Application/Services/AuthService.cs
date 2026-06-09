using Akaru.Application.DTOs;
using Akaru.Application.Interfaces;
using Akaru.Domain.Entities;
using Akaru.Domain.Exceptions;

namespace Akaru.Application.Services;

public class AuthService
{
    private readonly IUsuarioRepository _repository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUsuarioRepository repository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _repository = repository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> RegistrarAsync(RegisterDto dto, CancellationToken ct = default)
    {
        ValidarSenha(dto.Senha);

        if (await _repository.ObterPorEmailAsync(dto.Email, ct) is not null)
            throw new DomainException("E-mail já cadastrado.");

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = PasswordHasher.Hash(dto.Senha),
            FirebaseUid = $"jwt-{Guid.NewGuid():N}",
            DataCadastro = DateTime.UtcNow
        };

        var criado = await _repository.CriarAsync(usuario, ct);
        return CriarResposta(criado);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        var usuario = await _repository.ObterPorEmailAsync(dto.Email, ct)
            ?? throw new AcessoNegadoException("E-mail ou senha inválidos.");

        if (!PasswordHasher.Verificar(dto.Senha, usuario.Senha))
            throw new AcessoNegadoException("E-mail ou senha inválidos.");

        return CriarResposta(usuario);
    }

    private AuthResponseDto CriarResposta(Usuario usuario)
    {
        var (token, expiraEm) = _jwtTokenGenerator.GerarToken(usuario);
        return new AuthResponseDto(token, expiraEm, Mapear(usuario));
    }

    private static void ValidarSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha) || senha.Length < 6)
            throw new DomainException("Senha deve ter no mínimo 6 caracteres.");
    }

    private static UsuarioResponseDto Mapear(Usuario usuario) =>
        new(usuario.Id, usuario.Nome, usuario.Email, usuario.Latitude, usuario.Longitude,
            usuario.Cidade, usuario.Estado, usuario.DataCadastro);
}
