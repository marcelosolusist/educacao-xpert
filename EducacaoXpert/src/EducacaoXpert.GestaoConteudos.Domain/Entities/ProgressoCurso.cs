using EducacaoXpert.Core.DomainObjects;

namespace EducacaoXpert.GestaoConteudos.Domain.Entities;

public class ProgressoCurso : Entity
{
    public Guid CursoId { get; private set; }
    public Guid AlunoId { get; private set; }
    public int TotalAulas { get; private set; }
    public int AulasConcluidas { get; private set; }
    public int PercentualConcluido { get; private set; }
    public bool CursoConcluido => PercentualConcluido == 100;

    protected ProgressoCurso() { }

    public ProgressoCurso(Guid cursoId, Guid alunoId, int totalAulas)
    {
        CursoId = cursoId;
        AlunoId = alunoId;
        TotalAulas = totalAulas;
        AulasConcluidas = 0;
        PercentualConcluido = 0;
    }

    public void IncrementarProgresso()
    {
        if (AulasConcluidas < TotalAulas)
            AulasConcluidas++;

        AtualizarPercentualConcluido();
    }

    private void AtualizarPercentualConcluido()
    {
        PercentualConcluido = TotalAulas == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(AulasConcluidas / TotalAulas) * 100);
    }
}
