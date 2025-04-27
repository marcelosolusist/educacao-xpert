using Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class AulaAssistidaMapping : IEntityTypeConfiguration<AulaAssistida>
    {
        public void Configure(EntityTypeBuilder<AulaAssistida> entity)
        {
            entity.ToTable("AulasAssistidas");

            entity.HasKey(aulaAssistida => aulaAssistida.Id);

            entity.Property(aulaAssistida => aulaAssistida.UsuarioCriacaoId)
                  .HasColumnType("varchar(200)");

            entity.HasOne(aulaAssistida => aulaAssistida.Aula)
                  .WithMany(aula => aula.AulasAssistidas)
                  .HasForeignKey(aulaAssistida => aulaAssistida.AulaId);

            entity.HasOne(aulaAssistida => aulaAssistida.Matricula)
                  .WithMany(matricula => matricula.AulasAssistidas)
                  .HasForeignKey(aulaAssistida => aulaAssistida.MatriculaId);
        }
    }
}
