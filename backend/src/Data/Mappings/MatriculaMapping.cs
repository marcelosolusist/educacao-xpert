using Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class MatriculaMapping : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> entity)
        {
            entity.ToTable("Matriculas");

            entity.HasKey(matricula => matricula.Id);

            entity.Property(matricula => matricula.UsuarioCriacaoId)
                  .HasColumnType("varchar(200)");

            entity.HasOne(matricula => matricula.Curso)
                  .WithMany(curso => curso.Matriculas)
                  .HasForeignKey(matricula => matricula.CursoId);

            entity.HasOne(matricula => matricula.Aluno)
                  .WithMany(aluno => aluno.Matriculas)
                  .HasForeignKey(matricula => matricula.AlunoId);
        }
    }
}
