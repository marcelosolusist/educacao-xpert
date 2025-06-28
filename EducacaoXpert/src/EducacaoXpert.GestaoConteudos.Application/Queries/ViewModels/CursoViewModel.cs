namespace EducacaoXpert.GestaoConteudos.Application.Queries.ViewModels;

public class CursoViewModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Nome { get; set; } = string.Empty;
    public string ConteudoProgramatico { get; set; } = string.Empty;
    public int Preco { get; set; }
    public IEnumerable<AulaViewModel> Aulas { get; set; } = new List<AulaViewModel>();
}