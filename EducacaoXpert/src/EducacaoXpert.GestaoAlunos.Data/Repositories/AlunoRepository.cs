using EducacaoXpert.Core.Data;
using EducacaoXpert.Core.DomainObjects.Enums;
using EducacaoXpert.GestaoAlunos.Data.Context;
using EducacaoXpert.GestaoAlunos.Domain.Entities;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducacaoXpert.GestaoAlunos.Data.Repositories;

public class AlunoRepository(GestaoAlunosContext dbContext) : IAlunoRepository
{
    private readonly DbSet<Aluno> _dbSet = dbContext.Set<Aluno>();
    public IUnitOfWork UnitOfWork => dbContext;

    public async Task<Aluno?> ObterPorId(Guid alunoId)
    {
        return await _dbSet.FindAsync(alunoId);
    }
    public async Task<Matricula?> ObterMatriculaPorCursoEAlunoId(Guid cursoId, Guid alunoId)
    {
        return await dbContext.Set<Matricula>().AsNoTracking()
            .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
    }

    public async Task<IEnumerable<Matricula>> ObterMatriculasPendentePagamento(Guid alunoId)
    {
        return await dbContext.Set<Matricula>()
            .AsNoTracking()
            .Where(m => m.AlunoId == alunoId && m.Status == StatusMatricula.EmPagamento)
            .ToListAsync();
    }

    public async Task<Certificado?> ObterCertificadoPorId(Guid certificadoId, Guid alunoId)
    {
        return await dbContext.Set<Certificado>()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == certificadoId && c.AlunoId == alunoId);
    }

    public void Adicionar(Aluno aluno)
    {
        _dbSet.Add(aluno);
    }

    public void AdicionarCertificado(Certificado certificado)
    {
        dbContext.Set<Certificado>().Add(certificado);
    }

    public void AdicionarMatricula(Matricula matricula)
    {
        dbContext.Set<Matricula>().Add(matricula);
    }
    public void AtualizarMatricula(Matricula matricula)
    {
        dbContext.Set<Matricula>().Update(matricula);
    }
    public void Dispose()
    {
        dbContext.Dispose();
    }

}