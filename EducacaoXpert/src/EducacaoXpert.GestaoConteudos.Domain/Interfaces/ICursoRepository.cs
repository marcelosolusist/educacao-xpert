using EducacaoXpert.Core.Data;
using EducacaoXpert.GestaoConteudos.Domain.Entities;

namespace EducacaoXpert.GestaoConteudos.Domain.Interfaces;

public interface ICursoRepository : IRepository<Curso>
{
    Task<Curso?> ObterPorId(Guid cursoId);
    Task<Curso?> ObterCursoComAulas(Guid cursoId);
    Task<IEnumerable<Curso>> ObterTodos();
    Task<Aula?> ObterAulaPorId(Guid aulaId);
    void Adicionar(Curso curso);
    void Atualizar(Curso curso);
    void Remover(Curso curso);
    void AdicionarAula(Aula aula);
    void AtualizarAula(Aula aula);
}
