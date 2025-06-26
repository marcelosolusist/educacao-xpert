using EducacaoXpert.GestaoConteudos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducacaoXpert.GestaoConteudos.Data.Mappings;

public class ProgressoCursoMapping : IEntityTypeConfiguration<ProgressoCurso>
{
    public void Configure(EntityTypeBuilder<ProgressoCurso> builder)
    {
        builder.ToTable("ProgressoCursos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.AlunoId)
            .IsRequired();
        builder.Property(p => p.CursoId)
            .IsRequired();

        builder.Property(p => p.PercentualConcluido)
            .IsRequired();

        builder.Ignore(p => p.CursoConcluido);

        builder.HasIndex(p => new { p.CursoId, p.AlunoId })
            .IsUnique();
    }
}
