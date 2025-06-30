using EducacaoXpert.Core.Data;
using EducacaoXpert.Core.DomainObjects.Interfaces;
using EducacaoXpert.GestaoAlunos.Domain.Entities;

namespace EducacaoXpert.GestaoAlunos.Domain.Interfaces;

public interface IAlunoRepository : IRepository<Aluno>, IAggregateRoot
{
    Task<Aluno?> ObterPorId(Guid id);
    Task<Matricula?> ObterMatriculaPorCursoEAlunoId(Guid cursoId, Guid alunoId);
    Task<IEnumerable<Matricula>> ObterMatriculasEmPagamento(Guid alunoId);
    Task<Certificado?> ObterCertificadoPorId(Guid certificadoId, Guid alunoId);

    void AdicionarMatricula(Matricula matricula);
    void AtualizarMatricula(Matricula matricula);
    void Adicionar(Aluno aluno);
    void AdicionarCertificado(Certificado certificado);
}
