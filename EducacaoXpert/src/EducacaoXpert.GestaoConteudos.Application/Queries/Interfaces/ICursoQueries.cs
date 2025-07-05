using EducacaoXpert.GestaoConteudos.Application.Queries.DTO;

namespace EducacaoXpert.GestaoConteudos.Application.Queries.Interfaces;

public interface ICursoQueries
{
    Task<CursoDto?> ObterPorId(Guid cursoId);
    Task<IEnumerable<CursoDto>> ListarTodos();
    Task<IEnumerable<AulaDto>> ListarTodasAulasPorCursoId(Guid cursoId);
    Task<ProgressoCursoDto?> ObterProgressoCurso(Guid cursoId, Guid alunoId);
    Task<ProgressoAulaDto?> ObterProgressoAula(Guid aulaId, Guid alunoId);
    Task<AulaDto?> ObterAulaPorId(Guid aulaId);
}
