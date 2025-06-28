namespace EducacaoXpert.GestaoConteudos.Application.Queries.DTO;

public class CursoDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Nome { get; set; } = string.Empty;
    public string ConteudoProgramatico { get; set; } = string.Empty;
    public int Preco { get; set; }
    public IEnumerable<AulaDto> Aulas { get; set; } = new List<AulaDto>();
}