using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.Api.ViewModels;

public class MatriculaDetalhadaViewModel
{
    public Guid Id { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public StatusMatricula Status { get; set; }
    public DateTime? DataMatricula { get; set; }

}
