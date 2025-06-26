using EducacaoXpert.Core.Data;
using EducacaoXpert.GestaoConteudos.Domain.Entities;

namespace EducacaoXpert.GestaoConteudos.Domain.Interfaces;

public interface IAulaRepository : IRepository<Aula>
{
    Task<ProgressoAula?> ObterProgressoAula(Guid aulaId, Guid alunoId);
    void AdicionarProgressoAula(ProgressoAula progressoAula);
    void AtualizarProgressoAula(ProgressoAula progressoAula);
}
