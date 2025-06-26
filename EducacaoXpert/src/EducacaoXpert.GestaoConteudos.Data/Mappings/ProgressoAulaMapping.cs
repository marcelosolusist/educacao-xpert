using EducacaoXpert.GestaoConteudos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducacaoXpert.GestaoConteudos.Data.Mappings;

public class ProgressoAulaMapping : IEntityTypeConfiguration<ProgressoAula>
{
    public void Configure(EntityTypeBuilder<ProgressoAula> builder)
    {
        builder.ToTable("ProgressoAulas");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.AlunoId)
            .IsRequired();

        builder.Property(p => p.AulaId)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<short>();

        builder.HasIndex(p => new { p.AulaId, p.AlunoId })
            .IsUnique(); ;
    }
}