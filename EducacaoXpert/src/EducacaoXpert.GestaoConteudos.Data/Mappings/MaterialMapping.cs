using EducacaoXpert.GestaoConteudos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducacaoXpert.GestaoConteudos.Data.Mappings;

public class MaterialMapping : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materiais");

        builder.HasKey(m => m.Id);
    }
}
