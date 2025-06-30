using EducacaoXpert.PagamentoFaturamento.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducacaoXpert.PagamentoFaturamento.Data.Mappings;

public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.NomeCartao)
            .IsRequired()
            .HasColumnType("varchar(250)");

        builder.Property(c => c.NumeroCartaoMascarado)
            .IsRequired()
            .HasColumnType("varchar(16)");

        // 1 : 1 => Pagamento : Transacao
        builder.HasOne(c => c.Transacao)
            .WithOne(c => c.Pagamento);

        builder.ToTable("Pagamentos");
    }
}