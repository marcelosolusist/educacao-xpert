using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.Core.DomainObjects.DTO;

public class AulaDto
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string Nome { get; set; }
    public string Conteudo { get; set; }
    public StatusProgressoAula Status { get; set; }
}
