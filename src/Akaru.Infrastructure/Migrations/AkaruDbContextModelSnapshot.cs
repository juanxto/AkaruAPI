using System;
using Akaru.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Akaru.Infrastructure.Migrations
{
    [DbContext(typeof(AkaruDbContext))]
    partial class AkaruDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("AKARU")
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            modelBuilder.UseIdentityColumns();

            modelBuilder.Entity("Akaru.Domain.Entities.HistoricoRecomendacao", b =>
                {
                    b.Property<int>("Id").HasColumnType("NUMBER(10)").HasColumnName("ID");
                    b.Property<int?>("CulturaId").HasColumnType("NUMBER(10)").HasColumnName("ID_CULTURA");
                    b.Property<string>("CulturaNome").HasMaxLength(150).HasColumnType("VARCHAR2(150)").HasColumnName("NOME_CULTURA");
                    b.Property<string>("DadosClimaticos").HasMaxLength(4000).HasColumnType("VARCHAR2(4000)").HasColumnName("DADOS_CLIMA");
                    b.Property<DateTime>("DataGeracao").HasColumnType("TIMESTAMP(7)").HasColumnName("DT_GERACAO");
                    b.Property<string>("Detalhes").HasMaxLength(2000).HasColumnType("VARCHAR2(2000)").HasColumnName("DETALHES");
                    b.Property<decimal?>("Latitude").HasPrecision(10, 7).HasColumnType("NUMBER(10,7)").HasColumnName("LATITUDE");
                    b.Property<decimal?>("Longitude").HasPrecision(10, 7).HasColumnType("NUMBER(10,7)").HasColumnName("LONGITUDE");
                    b.Property<decimal?>("Score").HasPrecision(5, 2).HasColumnType("NUMBER(5,2)").HasColumnName("SCORE");
                    b.Property<string>("TextoRecomendacao").IsRequired().HasColumnType("CLOB").HasColumnName("TEXTO");
                    b.Property<int>("UsuarioId").HasColumnType("NUMBER(10)").HasColumnName("ID_USUARIO");
                    b.HasKey("Id");
                    b.HasIndex("UsuarioId");
                    b.ToTable("TB_HISTORICO_RECOMENDACAO", "AKARU");
                });

            modelBuilder.Entity("Akaru.Domain.Entities.Plantio", b =>
                {
                    b.Property<int>("Id").HasColumnType("NUMBER(10)").HasColumnName("ID");
                    b.Property<string>("Cidade").HasMaxLength(100).HasColumnType("VARCHAR2(100)").HasColumnName("CIDADE");
                    b.Property<int>("CulturaId").HasColumnType("NUMBER(10)").HasColumnName("ID_CULTURA");
                    b.Property<DateTime>("DataPlantio").HasColumnType("TIMESTAMP(7)").HasColumnName("DT_PLANTIO");
                    b.Property<DateTime>("DataRegistro").HasColumnType("TIMESTAMP(7)").HasColumnName("DT_REGISTRO");
                    b.Property<string>("Detalhes").HasMaxLength(2000).HasColumnType("VARCHAR2(2000)").HasColumnName("DETALHES");
                    b.Property<string>("Estado").HasMaxLength(2).HasColumnType("VARCHAR2(2)").HasColumnName("ESTADO");
                    b.Property<decimal>("Latitude").HasPrecision(10, 7).HasColumnType("NUMBER(10,7)").HasColumnName("LATITUDE");
                    b.Property<decimal>("Longitude").HasPrecision(10, 7).HasColumnType("NUMBER(10,7)").HasColumnName("LONGITUDE");
                    b.Property<int>("UsuarioId").HasColumnType("NUMBER(10)").HasColumnName("ID_USUARIO");
                    b.HasKey("Id");
                    b.HasIndex("UsuarioId");
                    b.ToTable("TB_PLANTIO", "AKARU");
                });

            modelBuilder.Entity("Akaru.Domain.Entities.PlantioCultura", b =>
                {
                    b.Property<int>("PlantioId").HasColumnType("NUMBER(10)").HasColumnName("ID_PLANTIO");
                    b.Property<int>("CulturaId").HasColumnType("NUMBER(10)").HasColumnName("ID_CULTURA");
                    b.HasKey("PlantioId", "CulturaId");
                    b.ToTable("TB_PLANTIO_CULTURA", "AKARU");
                });

            modelBuilder.Entity("Akaru.Domain.Entities.Usuario", b =>
                {
                    b.Property<int>("Id").HasColumnType("NUMBER(10)").HasColumnName("ID");
                    b.Property<string>("Cidade").HasMaxLength(100).HasColumnType("VARCHAR2(100)").HasColumnName("CIDADE");
                    b.Property<DateTime>("DataCadastro").HasColumnType("TIMESTAMP(7)").HasColumnName("DT_CADASTRO");
                    b.Property<string>("Email").IsRequired().HasMaxLength(200).HasColumnType("VARCHAR2(200)").HasColumnName("EMAIL");
                    b.Property<string>("Estado").HasMaxLength(2).HasColumnType("VARCHAR2(2)").HasColumnName("ESTADO");
                    b.Property<string>("FirebaseUid").IsRequired().HasMaxLength(128).HasColumnType("VARCHAR2(128)").HasColumnName("FIREBASE_UID");
                    b.Property<decimal?>("Latitude").HasPrecision(10, 7).HasColumnType("NUMBER(10,7)").HasColumnName("LATITUDE");
                    b.Property<decimal?>("Longitude").HasPrecision(10, 7).HasColumnType("NUMBER(10,7)").HasColumnName("LONGITUDE");
                    b.Property<string>("Nome").IsRequired().HasMaxLength(150).HasColumnType("VARCHAR2(150)").HasColumnName("NOME");
                    b.HasKey("Id");
                    b.HasIndex("Email").IsUnique();
                    b.HasIndex("FirebaseUid").IsUnique();
                    b.ToTable("TB_USUARIO", "AKARU");
                });

            modelBuilder.Entity("Akaru.Domain.Entities.HistoricoRecomendacao", b =>
                {
                    b.HasOne("Akaru.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Historicos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Akaru.Domain.Entities.Plantio", b =>
                {
                    b.HasOne("Akaru.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Plantios")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Akaru.Domain.Entities.PlantioCultura", b =>
                {
                    b.HasOne("Akaru.Domain.Entities.Plantio", "Plantio")
                        .WithMany("PlantioCulturas")
                        .HasForeignKey("PlantioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.Navigation("Plantio");
                });

            modelBuilder.Entity("Akaru.Domain.Entities.Plantio", b =>
                {
                    b.Navigation("PlantioCulturas");
                });

            modelBuilder.Entity("Akaru.Domain.Entities.Usuario", b =>
                {
                    b.Navigation("Historicos");
                    b.Navigation("Plantios");
                });
#pragma warning restore 612, 618
        }
    }
}
