using EducacaoXpert.Core.Data;
using EducacaoXpert.GestaoConteudos.Domain.Entities;

namespace EducacaoXpert.GestaoConteudos.Domain.Interfaces;

public interface IProgressoCursoRepository : IRepository<ProgressoCurso>
{
    Task<ProgressoCurso?> Obter(Guid cursoId, Guid alunoId);
    void Adicionar(ProgressoCurso progressoCurso);
    void Atualizar(ProgressoCurso progressoCurso);
    Task<ProgressoAula?> ObterProgressoAula(Guid aulaId, Guid alunoId);
    void AdicionarProgressoAula(ProgressoAula progressoAula);
    void AtualizarProgressoAula(ProgressoAula progressoAula);
}
