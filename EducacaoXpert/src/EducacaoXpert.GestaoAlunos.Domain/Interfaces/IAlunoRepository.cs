using EducacaoXpert.Core.Data;
using EducacaoXpert.Core.DomainObjects.Interfaces;
using EducacaoXpert.GestaoAlunos.Domain.Entities;

namespace EducacaoXpert.GestaoAlunos.Domain.Interfaces;

public interface IAlunoRepository : IRepository<Aluno>, IAggregateRoot
{
    Task<Aluno?> ObterPorId(Guid id);
    Task<Matricula?> ObterMatriculaPorCursoEAlunoId(Guid cursoId, Guid alunoId);
    Task<IEnumerable<Matricula>> ObterMatriculasAPagar(Guid alunoId);
    Task<Certificado?> ObterCertificadoPorId(Guid certificadoId, Guid alunoId);
    Task<IEnumerable<Certificado>> ListarCertificadosPorAlunoId(Guid alunoId);

    void IncluirMatricula(Matricula matricula);
    void EditarMatricula(Matricula matricula);
    void Incluir(Aluno aluno);
    void IncluirCertificado(Certificado certificado);
}
