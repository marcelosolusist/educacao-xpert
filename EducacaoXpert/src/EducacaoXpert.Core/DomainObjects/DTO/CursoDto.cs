namespace EducacaoXpert.Core.DomainObjects.DTO;

public class CursoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public List<AulaDto> Aulas { get; set; }
}
