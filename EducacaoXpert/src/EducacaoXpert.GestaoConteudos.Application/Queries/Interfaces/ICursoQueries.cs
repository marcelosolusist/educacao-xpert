using EducacaoXpert.GestaoConteudos.Application.Queries.ViewModels;

namespace EducacaoXpert.GestaoConteudos.Application.Queries.Interfaces;

public interface ICursoQueries
{
    Task<CursoViewModel?> ObterPorId(Guid cursoId);
    Task<IEnumerable<CursoViewModel>> ObterTodos();
}
