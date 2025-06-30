using EducacaoXpert.GestaoAlunos.Application.Queries.DTO;

namespace EducacaoXpert.GestaoAlunos.Application.Queries;

public interface IAlunoQueries
{
    Task<MatriculaDto?> ObterMatricula(Guid cursoId, Guid alunoId);
    Task<IEnumerable<MatriculaDto>> ObterMatriculasEmPagamento(Guid alunoId);
    Task<CertificadoDto> ObterCertificado(Guid certificadoId, Guid alunoId);
}
