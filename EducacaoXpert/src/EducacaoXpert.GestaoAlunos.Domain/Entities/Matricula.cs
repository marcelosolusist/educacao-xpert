using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.GestaoAlunos.Domain.Entities;

public class Matricula : Entity
{
    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public DateTime? DataMatricula { get; private set; }
    public StatusMatricula Status { get; private set; }

    // Ef relationship
    public Aluno Aluno { get; set; }

    // Ef Constructor
    protected Matricula() { }
    public Matricula(Guid alunoId, Guid cursoId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        Validar();
        Iniciar();
    }

    public void Iniciar()
    {
        Status = StatusMatricula.Pendente;
    }
    public void Ativar()
    {
        Status = StatusMatricula.Ativa;
        DataMatricula = DateTime.Now;
    }
    public void AguardarPagamento()
    {
        Status = StatusMatricula.APagar;
    }

    private void Validar()
    {
        if (AlunoId == Guid.Empty)
            throw new DomainException("O campo AlunoId é obrigatório.");
        if (CursoId == Guid.Empty)
            throw new DomainException("O campo CursoId é obrigatório.");
    }

}
