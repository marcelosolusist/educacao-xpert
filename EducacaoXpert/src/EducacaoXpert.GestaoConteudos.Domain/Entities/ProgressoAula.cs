using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.GestaoConteudos.Domain.Entities;

public class ProgressoAula : Entity
{
    public Guid ProgressoCursoId { get; private set; }
    public Guid AulaId { get; private set; }
    public StatusProgressoAula Status { get; private set; }
    public bool Assistindo { get; private set; }

    // EF Rel.
    public ProgressoCurso ProgressoCurso { get; set; }
    public Aula Aula { get; set; }

    protected ProgressoAula() { }

    public ProgressoAula(Guid aulaId)
    {
        AulaId = aulaId;
        Status = StatusProgressoAula.Iniciada;
        Validar();
    }
    internal void FinalizarAula() => Status = StatusProgressoAula.Finalizada;
    internal void MarcarAssistindo() => Assistindo = true;
    internal void DesmarcarAssistindo() => Assistindo = false;
    internal void AssociarProgressoCurso(Guid progressoCursoId)
    {
        ProgressoCursoId = progressoCursoId;
    }
    internal void Validar()
    {
        if (AulaId == Guid.Empty)
            throw new DomainException("O ID da aula não pode ser vazio.");
    }
}