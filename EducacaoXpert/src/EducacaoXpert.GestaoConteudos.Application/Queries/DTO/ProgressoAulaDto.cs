using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.GestaoConteudos.Application.Queries.DTO;

public class ProgressoAulaDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid ProgressoCursoId { get; set; } = Guid.Empty;
    public Guid AulaId { get; set; } = Guid.Empty;
    public StatusProgressoAula Status { get; set; } = StatusProgressoAula.Iniciada;
    public bool Assistindo { get; set; } = false;
}
