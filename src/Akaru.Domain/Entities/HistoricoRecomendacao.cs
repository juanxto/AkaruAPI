namespace Akaru.Domain.Entities;

public class HistoricoRecomendacao
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int? CulturaId { get; set; }
    public string? CulturaNome { get; set; }
    public string TextoRecomendacao { get; set; } = string.Empty;
    public decimal? Score { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Detalhes { get; set; }
    public string? DadosClimaticos { get; set; }
    public DateTime DataGeracao { get; set; } = DateTime.UtcNow;

    public Usuario Usuario { get; set; } = null!;
}
