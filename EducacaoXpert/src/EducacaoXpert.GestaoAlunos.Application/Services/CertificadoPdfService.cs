using EducacaoXpert.GestaoAlunos.Domain.Entities;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EducacaoXpert.GestaoAlunos.Application.Services;

public class CertificadoPdfService : ICertificadoPdfService
{
    public byte[] GerarPdf(Certificado certificado)
    {
        using var memoryStream = new MemoryStream();

        var document = new Document(PageSize.A4.Rotate(), 50, 50, 50, 50);
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        var tituloFonte = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 44);
        var titulo = new Paragraph("CERTIFICADO", tituloFonte)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 150f
        };
        document.Add(titulo);

        var corpoFonte = FontFactory.GetFont(FontFactory.HELVETICA, 22);
        var corpo = new Paragraph(certificado.Descricao, corpoFonte)
        {
            Alignment = Element.ALIGN_CENTER
        };
        document.Add(corpo);

        var dataFonte = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 16);
        var dataTexto = new Paragraph($"Emitido em {DateTime.Now:dd/MM/yyyy} às {DateTime.Now:HH:mm:ss}.", dataFonte)
        {
            Alignment = Element.ALIGN_RIGHT,
            SpacingBefore = 150f
        };
        document.Add(dataTexto);

        document.Close();
        return memoryStream.ToArray();
    }
}

