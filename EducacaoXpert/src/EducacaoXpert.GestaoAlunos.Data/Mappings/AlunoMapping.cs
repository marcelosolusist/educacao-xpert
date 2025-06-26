using EducacaoXpert.GestaoAlunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducacaoXpert.GestaoAlunos.Data.Mappings;

public class AlunoMapping : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.ToTable("Alunos");

        builder.HasMany(a => a.Matriculas)
            .WithOne(m => m.Aluno)
            .HasForeignKey(m => m.AlunoId);

        builder.HasMany(a => a.Certificados)
            .WithOne(c => c.Aluno);
    }
}
