namespace Akaru.Domain.Entities;

public class Plantio
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int CulturaId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public DateTime DataPlantio { get; set; }
    public string? Detalhes { get; set; }
    public DateTime DataRegistro { get; set; } = DateTime.UtcNow;

    public Usuario Usuario { get; set; } = null!;
    public ICollection<PlantioCultura> PlantioCulturas { get; set; } = new List<PlantioCultura>();
}
