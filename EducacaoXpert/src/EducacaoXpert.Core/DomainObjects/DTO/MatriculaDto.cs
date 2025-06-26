using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.Core.DomainObjects.DTO;

public class MatriculaDto
{
    public Guid Id { get; set; }
    public StatusMatricula Status { get; set; }
}
