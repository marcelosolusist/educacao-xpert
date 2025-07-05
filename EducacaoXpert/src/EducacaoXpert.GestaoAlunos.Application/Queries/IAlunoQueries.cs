using EducacaoXpert.GestaoAlunos.Application.Queries.DTO;

namespace EducacaoXpert.GestaoAlunos.Application.Queries;

public interface IAlunoQueries
{
    Task<MatriculaDto?> ObterMatricula(Guid cursoId, Guid alunoId);
    Task<IEnumerable<MatriculaDto>> ObterMatriculasAPagar(Guid alunoId);
    Task<ArquivoCertificadoDto> ObterCertificado(Guid certificadoId, Guid alunoId);
    Task<IEnumerable<CertificadoDto>> ListarCertificados(Guid alunoId);
}
