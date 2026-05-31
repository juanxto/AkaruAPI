using Akaru.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Akaru.Infrastructure.Persistence;

public class AkaruDbContext : DbContext
{
    public AkaruDbContext(DbContextOptions<AkaruDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Plantio> Plantios => Set<Plantio>();
    public DbSet<PlantioCultura> PlantioCulturas => Set<PlantioCultura>();
    public DbSet<HistoricoRecomendacao> Historicos => Set<HistoricoRecomendacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("AKARU");

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("TB_USUARIO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirebaseUid).HasColumnName("FIREBASE_UID").HasMaxLength(128).IsRequired();
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Latitude).HasColumnName("LATITUDE").HasPrecision(10, 7);
            entity.Property(e => e.Longitude).HasColumnName("LONGITUDE").HasPrecision(10, 7);
            entity.Property(e => e.Cidade).HasColumnName("CIDADE").HasMaxLength(100);
            entity.Property(e => e.Estado).HasColumnName("ESTADO").HasMaxLength(2);
            entity.Property(e => e.DataCadastro).HasColumnName("DT_CADASTRO");
            entity.HasIndex(e => e.FirebaseUid).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Plantio>(entity =>
        {
            entity.ToTable("TB_PLANTIO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UsuarioId).HasColumnName("ID_USUARIO");
            entity.Property(e => e.CulturaId).HasColumnName("ID_CULTURA");
            entity.Property(e => e.Latitude).HasColumnName("LATITUDE").HasPrecision(10, 7);
            entity.Property(e => e.Longitude).HasColumnName("LONGITUDE").HasPrecision(10, 7);
            entity.Property(e => e.Cidade).HasColumnName("CIDADE").HasMaxLength(100);
            entity.Property(e => e.Estado).HasColumnName("ESTADO").HasMaxLength(2);
            entity.Property(e => e.DataPlantio).HasColumnName("DT_PLANTIO");
            entity.Property(e => e.Detalhes).HasColumnName("DETALHES").HasMaxLength(2000);
            entity.Property(e => e.DataRegistro).HasColumnName("DT_REGISTRO");

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Plantios)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PlantioCultura>(entity =>
        {
            entity.ToTable("TB_PLANTIO_CULTURA");
            entity.HasKey(e => new { e.PlantioId, e.CulturaId });
            entity.Property(e => e.PlantioId).HasColumnName("ID_PLANTIO");
            entity.Property(e => e.CulturaId).HasColumnName("ID_CULTURA");

            entity.HasOne(e => e.Plantio)
                .WithMany(p => p.PlantioCulturas)
                .HasForeignKey(e => e.PlantioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HistoricoRecomendacao>(entity =>
        {
            entity.ToTable("TB_HISTORICO_RECOMENDACAO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UsuarioId).HasColumnName("ID_USUARIO");
            entity.Property(e => e.CulturaId).HasColumnName("ID_CULTURA");
            entity.Property(e => e.CulturaNome).HasColumnName("NOME_CULTURA").HasMaxLength(150);
            entity.Property(e => e.TextoRecomendacao).HasColumnName("TEXTO").HasColumnType("CLOB");
            entity.Property(e => e.Score).HasColumnName("SCORE").HasPrecision(5, 2);
            entity.Property(e => e.Latitude).HasColumnName("LATITUDE").HasPrecision(10, 7);
            entity.Property(e => e.Longitude).HasColumnName("LONGITUDE").HasPrecision(10, 7);
            entity.Property(e => e.Detalhes).HasColumnName("DETALHES").HasMaxLength(2000);
            entity.Property(e => e.DadosClimaticos).HasColumnName("DADOS_CLIMA").HasMaxLength(4000);
            entity.Property(e => e.DataGeracao).HasColumnName("DT_GERACAO");

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Historicos)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
