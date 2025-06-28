using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.GestaoAlunos.Application.Queries.DTO;

public class MatriculaDto
{
    public Guid Id { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public StatusMatricula Status { get; set; }
    public DateTime? DataMatricula { get; set; }
}
