using Business.Entities;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class AlunoMapping : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> entity)
        {
            entity.ToTable("Alunos");

            entity.HasKey(aluno => aluno.Id);

            entity.Property(aluno => aluno.Id)
                  .HasColumnType("varchar(200)");

            entity.Property(aluno => aluno.Nome)
                  .HasColumnType("varchar(200)");

            entity.Property(aluno => aluno.UsuarioCriacaoId)
                  .HasColumnType("varchar(200)");
        }
    }
}
