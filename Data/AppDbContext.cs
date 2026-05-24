using Challenge_Clyvo_Vet_DotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace Challenge_Clyvo_Vet_DotNet.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pet> Pets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>(entity =>
        {
            // Tabela
            entity.ToTable("T_CLV_PET");

            // Chave primária
            entity.HasKey(p => p.IdPet);

            // Constraint de PK
            entity.HasIndex(p => p.IdPet)
                  .HasDatabaseName("T_CLV_PET_ID_PET_PK")
                  .IsUnique();

            // Colunas
            entity.Property(p => p.IdPet).HasColumnName("ID_PET");
            entity.Property(p => p.IdResponsavel).HasColumnName("ID_RESPONSAVEL").IsRequired();
            entity.Property(p => p.NomePet).HasColumnName("NOME_PET").HasMaxLength(100).IsRequired();
            entity.Property(p => p.EspeciePet).HasColumnName("ESPECIE_PET").HasMaxLength(100).IsRequired();
            entity.Property(p => p.RacaPet).HasColumnName("RACA_PET").HasMaxLength(100).IsRequired();
            entity.Property(p => p.DataNascimentoPet).HasColumnName("DATA_NASCIMENTO_PET").IsRequired();
            entity.Property(p => p.StatusCastrado).HasColumnName("STATUS_CASTRADO").HasMaxLength(1).IsFixedLength();

            //TODO: Habilitar assim que o CRUD de Responsável estiver pronto
            // entity.HasIndex(p => p.IdResponsavel)
            //       .HasDatabaseName("T_CLV_PET_T_CLV_RESPONSAVEL_FK");
        });
    }
}