using EducacaoXpert.GestaoAlunos.Application.Queries.ViewModels;

namespace EducacaoXpert.GestaoAlunos.Application.Queries;

public interface IAlunoQueries
{
    Task<MatriculaViewModel?> ObterMatricula(Guid cursoId, Guid alunoId);
    Task<IEnumerable<MatriculaViewModel>> ObterMatriculasPendentePagamento(Guid alunoId);
    Task<CertificadoViewModel> ObterCertificado(Guid certificadoId, Guid alunoId);
}
