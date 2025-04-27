using Business.Entities;
using Business.Interfaces;
using Data.Context;

namespace Data.Repositories
{
    public class AlunoRepository(AppDbContext dbContext) : Repository<Aluno>(dbContext), IAlunoRepository
    {
    }
}
