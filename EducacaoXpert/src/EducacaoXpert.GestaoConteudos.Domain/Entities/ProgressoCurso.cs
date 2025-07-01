using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Enums;
using EducacaoXpert.Core.DomainObjects.Interfaces;

namespace EducacaoXpert.GestaoConteudos.Domain.Entities;

public class ProgressoCurso : Entity, IAggregateRoot
{
    public Guid CursoId { get; private set; }
    public Guid AlunoId { get; private set; }
    public int TotalAulas { get; private set; }
    public int AulasFinalizadas { get; private set; }
    public int PercentualConcluido { get; private set; }
    public bool CertificadoGerado { get; private set; }
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
        AulasFinalizadas = 0;
        PercentualConcluido = 0;
        CertificadoGerado = false;
        _progressoAulas = new List<ProgressoAula>();
    }

    public void IncluirProgressoAula(ProgressoAula progressoAula)
    {
        if (ProgressoAulaExistente(progressoAula))
            throw new DomainException("Progresso de aula já associada ao progresso do curso."); 
        progressoAula.Validar();
        progressoAula.AssociarProgressoCurso(Id);
        _progressoAulas.Add(progressoAula);
    }
    public void FinalizarProgressoAula(ProgressoAula progressoAula)
    {
        var aulaMarcar = _progressoAulas.FirstOrDefault(p => p.Id == progressoAula.Id);
        if (aulaMarcar is null)
            throw new DomainException("Progresso de aula não existe.");
        aulaMarcar.FinalizarAula();
        AtualizarAulasFinalizadas();
    }

    public void MarcarCertificadoGerado()
    {
        CertificadoGerado = true;
    }

    private bool ProgressoAulaExistente(ProgressoAula progressoAula)
    {
        return _progressoAulas.Any(p => p.AulaId == progressoAula.AulaId);
    }

    private void AtualizarAulasFinalizadas()
    {
        AulasFinalizadas = _progressoAulas.Count(p => p.Status == StatusProgressoAula.Finalizada);
        AtualizarPercentualConcluido();
    }

    private void AtualizarPercentualConcluido()
    {
        PercentualConcluido = TotalAulas == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(AulasFinalizadas / TotalAulas) * 100);
    }
}
