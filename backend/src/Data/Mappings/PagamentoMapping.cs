using Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> entity)
        {
            entity.ToTable("Pagamentos");

            entity.HasKey(pagamento => pagamento.Id);

            entity.Property(pagamento => pagamento.UsuarioCriacaoId)
                  .HasColumnType("varchar(200)");

            entity.HasOne(pagamento => pagamento.Matricula)
                  .WithMany(matricula => matricula.Pagamentos)
                  .HasForeignKey(pagamento => pagamento.MatriculaId);
        }
    }
}
