namespace EducacaoXpert.Core.DomainObjects.DTO;

public class PagamentoCurso
{
    public Guid CursoId { get; set; }
    public Guid AlunoId { get; set; }
    public int Total { get; set; }
    public string NomeCartao { get; set; }
    public string NumeroCartao { get; set; }
    public string ExpiracaoCartao { get; set; }
    public string CvvCartao { get; set; }
}
