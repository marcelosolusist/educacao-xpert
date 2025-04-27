using Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class CertificadoMapping : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> entity)
        {
            entity.ToTable("Certificados");

            entity.HasKey(certificado => certificado.Id);

            entity.Property(certificado => certificado.Conteudo)
                  .HasColumnType("varchar(500)");

            entity.Property(certificado => certificado.UsuarioCriacaoId)
                  .HasColumnType("varchar(200)");
        }
    }
}
