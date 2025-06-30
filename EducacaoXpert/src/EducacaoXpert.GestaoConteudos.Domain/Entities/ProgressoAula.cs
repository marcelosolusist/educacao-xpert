using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.GestaoConteudos.Domain.Entities;

public class ProgressoAula : Entity
{
    public Guid ProgressoCursoId { get; private set; }
    public Guid AulaId { get; private set; }
    public StatusProgressoAula Status { get; private set; }

    // EF Rel.
    public ProgressoCurso ProgressoCurso { get; set; }
    public Aula Aula { get; set; }

    protected ProgressoAula() { }

    public ProgressoAula(Guid aulaId)
    {
        Validar();
        AulaId = aulaId;
        Status = StatusProgressoAula.Iniciada;
    }
    internal void FinalizarAula() => Status = StatusProgressoAula.Finalizada;

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