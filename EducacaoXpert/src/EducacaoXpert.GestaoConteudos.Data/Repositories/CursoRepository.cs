using EducacaoXpert.Core.Data;
using EducacaoXpert.GestaoConteudos.Data.Context;
using EducacaoXpert.GestaoConteudos.Domain.Entities;
using EducacaoXpert.GestaoConteudos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducacaoXpert.GestaoConteudos.Data.Repositories;

public class CursoRepository(GestaoConteudosContext dbContext) : ICursoRepository
{
    private readonly DbSet<Curso> _dbSet = dbContext.Set<Curso>();
    public IUnitOfWork UnitOfWork => dbContext;

    public async Task<Curso?> ObterCursoComAulas(Guid cursoId)
    {
        return await _dbSet.AsNoTracking()
            .Include(c => c.Aulas)
            .FirstOrDefaultAsync(c => c.Id == cursoId);
    }
    public async Task<Curso?> ObterPorId(Guid id)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<IEnumerable<Curso>> ListarTodos()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }
    public async Task<Aula?> ObterAulaPorId(Guid aulaId)
    {
        return await dbContext.Set<Aula>()
            .Include(a => a.Materiais)
            .FirstOrDefaultAsync(a => a.Id == aulaId);
    }
    public async Task<ProgressoCurso?> ObterProgressoCurso(Guid cursoId, Guid alunoId)
    {
        return await dbContext.Set<ProgressoCurso>().AsNoTracking()
             .FirstOrDefaultAsync(pc => pc.CursoId == cursoId && pc.AlunoId == alunoId);
    }
    public async Task<IEnumerable<Aula>> ListarTodasAulasPorCursoId(Guid cursoId)
    {
        return await dbContext.Set<Aula>().AsNoTracking().Where(a => a.CursoId == cursoId).ToListAsync();
    }
    public void Incluir(Curso curso)
    {
        _dbSet.Add(curso);
    }
    public void Editar(Curso curso)
    {
        _dbSet.Update(curso);
    }
    public void Remover(Curso curso)
    {
        _dbSet.Remove(curso);
    }
    public void IncluirAula(Aula aula)
    {
        dbContext.Set<Aula>().Add(aula);
    }
    public void EditarAula(Aula aula)
    {
        dbContext.Set<Aula>().Update(aula);
    }
    public void RemoverAula(Aula aula)
    {
        dbContext.Set<Material>().RemoveRange(aula.Materiais);
        dbContext.Set<Aula>().Remove(aula);
    }
    public void Dispose()
    {
        dbContext.Dispose();
    }
}
