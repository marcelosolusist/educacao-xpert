using EducacaoXpert.GestaoConteudos.Application.Queries.DTO;

namespace EducacaoXpert.GestaoConteudos.Application.Queries.Interfaces;

public interface ICursoQueries
{
    Task<CursoDto?> ObterPorId(Guid cursoId);
    Task<IEnumerable<CursoDto>> ListarTodos();
    Task<IEnumerable<AulaDto>> ListarTodasAulasPorCursoId(Guid cursoId);
}
