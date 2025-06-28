namespace EducacaoXpert.GestaoConteudos.Application.Queries.DTO;

public class AulaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public IEnumerable<MaterialDto> Materiais { get; set; } = new List<MaterialDto>();
    public string Status { get; set; } = string.Empty;
}