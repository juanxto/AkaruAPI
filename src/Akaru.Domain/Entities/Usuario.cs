namespace Akaru.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string FirebaseUid { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Senha { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public ICollection<Plantio> Plantios { get; set; } = new List<Plantio>();
    public ICollection<HistoricoRecomendacao> Historicos { get; set; } = new List<HistoricoRecomendacao>();
}
