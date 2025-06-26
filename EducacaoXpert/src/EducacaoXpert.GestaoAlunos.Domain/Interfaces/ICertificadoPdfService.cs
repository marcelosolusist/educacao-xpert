using EducacaoXpert.GestaoAlunos.Domain.Entities;

namespace EducacaoXpert.GestaoAlunos.Domain.Interfaces;

public interface ICertificadoPdfService
{
    byte[] GerarPdf(Certificado certificado);
}
