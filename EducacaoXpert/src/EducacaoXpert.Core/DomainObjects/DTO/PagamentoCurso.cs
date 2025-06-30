namespace EducacaoXpert.Core.DomainObjects.DTO;

public class PagamentoCurso
{
    public Guid? PagamentoId { get; set; }
    public Guid CursoId { get; set; }
    public Guid AlunoId { get; set; }
    public int Valor { get; set; }
    public string NomeCartao { get; set; }
    public string NumeroCartao { get; set; }
    public string ExpiracaoCartao { get; set; }
    public string CvvCartao { get; set; }
}
