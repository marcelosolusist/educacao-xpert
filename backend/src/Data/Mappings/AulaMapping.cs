using Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class AulaMapping : IEntityTypeConfiguration<Aula>
    {
        public void Configure(EntityTypeBuilder<Aula> entity)
        {
            entity.ToTable("Aulas");

            entity.HasKey(aula => aula.Id);

            entity.Property(aula => aula.Titulo)
                  .HasColumnType("varchar(200)");

            entity.Property(aula => aula.Conteudo)
                  .HasColumnType("varchar(500)");

            entity.Property(aula => aula.UsuarioCriacaoId)
                  .HasColumnType("varchar(200)");

            entity.HasOne(aula => aula.Curso)
                  .WithMany(curso => curso.Aulas)
                  .HasForeignKey(aula => aula.CursoId);
        }
    }
}
