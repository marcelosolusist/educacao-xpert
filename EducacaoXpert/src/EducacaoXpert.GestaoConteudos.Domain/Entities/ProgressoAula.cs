using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.GestaoConteudos.Domain.Entities;

public class ProgressoAula : Entity
{
    public Guid AlunoId { get; private set; }
    public Guid AulaId { get; private set; }
    public StatusProgressoAula Status { get; private set; }

    protected ProgressoAula() { }

    public ProgressoAula(Guid alunoId, Guid aulaId)
    {
        AlunoId = alunoId;
        AulaId = aulaId;
        Status = StatusProgressoAula.Pendente;
        Validar();
    }
    public void EmAndamento() => Status = StatusProgressoAula.Iniciada;
    public void ConcluirAula() => Status = StatusProgressoAula.Assistida;

    public void Validar()
    {
        if (AlunoId == Guid.Empty)
            throw new DomainException("O ID do aluno não pode ser vazio.");
        if (AulaId == Guid.Empty)
            throw new DomainException("O ID da aula não pode ser vazio.");
    }
}