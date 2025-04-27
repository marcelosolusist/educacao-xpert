using Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> entity)
        {
            entity.ToTable("Cursos");

            entity.HasKey(curso => curso.Id);

            entity.Property(curso => curso.Titulo)
                  .HasColumnType("varchar(200)");

            entity.Property(curso => curso.Instrutor)
                  .HasColumnType("varchar(200)");

            entity.Property(curso => curso.UsuarioCriacaoId)
                  .HasColumnType("varchar(200)");
        }
    }
}
