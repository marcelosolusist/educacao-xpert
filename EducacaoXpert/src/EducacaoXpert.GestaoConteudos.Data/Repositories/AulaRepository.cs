using Microsoft.EntityFrameworkCore;
using EducacaoXpert.Core.Data;
using EducacaoXpert.GestaoConteudos.Data.Context;
using EducacaoXpert.GestaoConteudos.Domain.Entities;
using EducacaoXpert.GestaoConteudos.Domain.Interfaces;

namespace EducacaoXpert.GestaoConteudos.Data.Repositories;

public class AulaRepository(GestaoConteudosContext dbContext) : IAulaRepository
{
    public IUnitOfWork UnitOfWork => dbContext;

    public void AdicionarProgressoAula(ProgressoAula progressoAula)
    {
        dbContext.Set<ProgressoAula>().Add(progressoAula);
    }
    public void AtualizarProgressoAula(ProgressoAula progressoAula)
    {
        dbContext.Set<ProgressoAula>().Update(progressoAula);
    }
    public async Task<ProgressoAula?> ObterProgressoAula(Guid aulaId, Guid alunoId)
    {
        return await dbContext.Set<ProgressoAula>().AsNoTracking()
            .FirstOrDefaultAsync(p => p.AulaId == aulaId && p.AlunoId == alunoId);
    }

    public void Dispose()
    {
        dbContext?.Dispose();
    }
}