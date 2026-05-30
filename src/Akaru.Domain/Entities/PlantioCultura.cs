namespace Akaru.Domain.Entities;

/// <summary>
/// Relacionamento N:N entre plantio e culturas (requisito FIAP).
/// </summary>
public class PlantioCultura
{
    public int PlantioId { get; set; }
    public int CulturaId { get; set; }

    public Plantio Plantio { get; set; } = null!;
}
