using EducacaoXpert.Core.Data;
using EducacaoXpert.GestaoConteudos.Domain.Entities;

namespace EducacaoXpert.GestaoConteudos.Domain.Interfaces;

public interface ICursoRepository : IRepository<Curso>
{
    Task<Curso?> ObterPorId(Guid cursoId);
    Task<Curso?> ObterCursoComAulas(Guid cursoId);
    Task<IEnumerable<Curso>> ListarTodos();
    Task<Aula?> ObterAulaPorId(Guid aulaId);
    Task<IEnumerable<Aula>> ListarTodasAulasPorCursoId(Guid cursoId);
    void Incluir(Curso curso);
    void Editar(Curso curso);
    void Remover(Curso curso);
    void IncluirAula(Aula aula);
    void EditarAula(Aula aula);
    void RemoverAula(Aula aula);
}
