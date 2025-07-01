namespace EducacaoXpert.Core.Messages.IntegrationEvents;

public class CursoConcluidoEvent : Event
{
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public string NomeCurso { get; set; }

    public CursoConcluidoEvent(Guid alunoId, Guid cursoId, string nomeCurso)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        NomeCurso = nomeCurso;
        AggregateId = alunoId;
    }
}
