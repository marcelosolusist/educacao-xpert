using EducacaoXpert.Core.Data;
using EducacaoXpert.GestaoConteudos.Domain.Entities;

namespace EducacaoXpert.GestaoConteudos.Domain.Interfaces;

public interface IProgressoCursoRepository : IRepository<ProgressoCurso>
{
    Task<ProgressoCurso?> Obter(Guid cursoId, Guid alunoId);
    void Incluir(ProgressoCurso progressoCurso);
    void Editar(ProgressoCurso progressoCurso);
    Task<ProgressoAula?> ObterProgressoAula(Guid aulaId, Guid alunoId);
    void IncluirProgressoAula(ProgressoAula progressoAula);
    void EditarProgressoAula(ProgressoAula progressoAula);
}
