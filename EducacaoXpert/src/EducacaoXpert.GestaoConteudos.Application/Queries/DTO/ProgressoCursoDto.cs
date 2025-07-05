namespace EducacaoXpert.GestaoConteudos.Application.Queries.DTO;

public class ProgressoCursoDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid CursoId { get; set; } = Guid.Empty;
    public Guid AlunoId { get; set; } = Guid.Empty;
    public int TotalAulas { get; set; } = 0;
    public int AulasFinalizadas { get; set; } = 0;
    public int PercentualConcluido { get; set; } = 0;
    public bool CertificadoGerado { get; set; } = false;
    public bool CursoConcluido => PercentualConcluido == 100;

    public List<ProgressoAulaDto> ProgressoAulas = new List<ProgressoAulaDto>();
}
