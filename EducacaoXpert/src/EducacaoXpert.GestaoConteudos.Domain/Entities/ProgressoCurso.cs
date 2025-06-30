using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Enums;
using EducacaoXpert.Core.DomainObjects.Interfaces;

namespace EducacaoXpert.GestaoConteudos.Domain.Entities;

public class ProgressoCurso : Entity, IAggregateRoot
{
    public Guid CursoId { get; private set; }
    public Guid AlunoId { get; private set; }
    public int TotalAulas { get; private set; }
    public int AulasAssistidas { get; private set; }
    public int PercentualConcluido { get; private set; }
    public bool CursoConcluido => PercentualConcluido == 100;

    private readonly List<ProgressoAula> _progressoAulas;
    public IReadOnlyCollection<ProgressoAula> ProgressoAulas => _progressoAulas;

    // EF Rel.
    public Curso Curso { get; set; }

    protected ProgressoCurso() { }

    public ProgressoCurso(Guid cursoId, Guid alunoId, int totalAulas)
    {
        CursoId = cursoId;
        AlunoId = alunoId;
        TotalAulas = totalAulas;
        AulasAssistidas = 0;
        PercentualConcluido = 0;
        _progressoAulas = new List<ProgressoAula>();
    }

    public void AdicionarProgressoAula(ProgressoAula progressoAula)
    {
        if (ProgressoAulaExistente(progressoAula))
            throw new DomainException("Progresso de aula já associada ao progresso do curso."); 
        progressoAula.Validar();
        progressoAula.AssociarProgressoCurso(Id);
        _progressoAulas.Add(progressoAula);
    }
    public void MarcarProgressoAulaAssistida(ProgressoAula progressoAula)
    {
        var aulaMarcar = _progressoAulas.FirstOrDefault(p => p.Id == progressoAula.Id);
        if (aulaMarcar is null)
            throw new DomainException("Progresso de aula não existe.");
        aulaMarcar.AtualizarAulaAssistida();
        AtualizarAulasAssistidas();
    }

    private bool ProgressoAulaExistente(ProgressoAula progressoAula)
    {
        return _progressoAulas.Any(p => p.AulaId == progressoAula.AulaId);
    }

    private void AtualizarAulasAssistidas()
    {
        AulasAssistidas = _progressoAulas.Count(p => p.Status == StatusProgressoAula.Assistida);
        AtualizarPercentualConcluido();
    }

    private void AtualizarPercentualConcluido()
    {
        PercentualConcluido = TotalAulas == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(AulasAssistidas / TotalAulas) * 100);
    }
}
