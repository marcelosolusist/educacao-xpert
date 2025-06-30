using EducacaoXpert.GestaoConteudos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducacaoXpert.GestaoConteudos.Data.Mappings;

public class CursoMapping : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");
        builder.HasKey(c => c.Id);

        // 1 : N - Curso : Aulas
        builder.HasMany(c => c.Aulas)
            .WithOne(c => c.Curso);

        // 1 : N - Curso : ProgressoCursos
        builder.HasMany(c => c.ProgressoCursos)
            .WithOne(c => c.Curso);
    }
}
