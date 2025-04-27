using Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class PagamentoHistoricoMapping : IEntityTypeConfiguration<PagamentoHistorico>
    {
        public void Configure(EntityTypeBuilder<PagamentoHistorico> entity)
        {
            entity.ToTable("PagamentosHistoricos");

            entity.HasKey(pagamentoHistorico => pagamentoHistorico.Id);

            entity.Property(pagamentoHistorico => pagamentoHistorico.AutorizacaoAdquirenteId)
                  .HasColumnType("varchar(200)");

            entity.Property(pagamentoHistorico => pagamentoHistorico.UsuarioCriacaoId)
                  .HasColumnType("varchar(200)");

            entity.HasOne(pagamentoHistorico => pagamentoHistorico.Pagamento)
                  .WithMany(pagamento => pagamento.PagamentosHistoricos)
                  .HasForeignKey(pagamentoHistorico => pagamentoHistorico.PagamentoId);
        }
    }
}
