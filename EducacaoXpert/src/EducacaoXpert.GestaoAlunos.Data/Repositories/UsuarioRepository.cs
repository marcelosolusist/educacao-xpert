using EducacaoXpert.Core.Data;
using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.GestaoAlunos.Data.Context;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducacaoXpert.GestaoAlunos.Data.Repositories;

public class UsuarioRepository(GestaoAlunosContext dbContext) : IUsuarioRepository
{
    private readonly DbSet<Usuario> _dbSet = dbContext.Set<Usuario>();
    public IUnitOfWork UnitOfWork => dbContext;
    public void Adicionar(Usuario usuario)
    {
        _dbSet.Add(usuario);
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}
