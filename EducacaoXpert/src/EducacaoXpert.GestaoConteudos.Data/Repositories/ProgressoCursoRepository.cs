using EducacaoXpert.Core.Data;
using EducacaoXpert.GestaoConteudos.Data.Context;
using EducacaoXpert.GestaoConteudos.Domain.Entities;
using EducacaoXpert.GestaoConteudos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducacaoXpert.GestaoConteudos.Data.Repositories;

public class ProgressoCursoRepository(GestaoConteudosContext dbContext) : IProgressoCursoRepository
{
    private readonly DbSet<ProgressoCurso> _dbSet = dbContext.Set<ProgressoCurso>();
    public IUnitOfWork UnitOfWork => dbContext;

    public void Incluir(ProgressoCurso progressoCurso)
    {
        _dbSet.Add(progressoCurso);
    }
    public void IncluirProgressoAula(ProgressoAula progressoAula)
    {
        dbContext.Set<ProgressoAula>().Add(progressoAula);
    }
    public void Editar(ProgressoCurso progressoCurso)
    {
        _dbSet.Update(progressoCurso);
    }
    public void EditarProgressoAula(ProgressoAula progressoAula)
    {
        dbContext.Set<ProgressoAula>().Update(progressoAula);
    }
    public async Task<ProgressoCurso?> Obter(Guid cursoId, Guid alunoId)
    {
        return await _dbSet.Include(pc => pc.ProgressoAulas)
            .Include(pc => pc.Curso)
            .FirstOrDefaultAsync(pc => pc.CursoId == cursoId && pc.AlunoId == alunoId);
    }
    public async Task<ProgressoAula?> ObterProgressoAula(Guid aulaId, Guid alunoId)
    {
        return await dbContext.Set<ProgressoAula>().AsNoTracking()
            .Include(p => p.ProgressoCurso)
            .Include(p => p.Aula)
            .FirstOrDefaultAsync(p => p.AulaId == aulaId && p.ProgressoCurso.AlunoId == alunoId);
    }
    public void Dispose()
    {
        dbContext.Dispose();
    }
}
