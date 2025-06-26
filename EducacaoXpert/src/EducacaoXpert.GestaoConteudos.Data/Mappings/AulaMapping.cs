using EducacaoXpert.GestaoConteudos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducacaoXpert.GestaoConteudos.Data.Mappings;

public class AulaMapping : IEntityTypeConfiguration<Aula>
{
    public void Configure(EntityTypeBuilder<Aula> builder)
    {
        builder.ToTable("Aulas");
        builder.HasKey(a => a.Id);

        // 1 : N - Aula : Materiais
        builder.HasMany(a => a.Materiais)
            .WithOne(a => a.Aula);
    }
}