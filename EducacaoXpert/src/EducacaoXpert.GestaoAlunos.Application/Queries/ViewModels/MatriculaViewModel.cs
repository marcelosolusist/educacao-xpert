using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.GestaoAlunos.Application.Queries.ViewModels;

public class MatriculaViewModel
{
    public Guid Id { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public StatusMatricula Status { get; set; }
    public DateTime? DataMatricula { get; set; }
}
