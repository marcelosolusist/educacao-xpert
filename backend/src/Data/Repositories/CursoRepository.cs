using Business.Entities;
using Business.Interfaces;
using Data.Context;

namespace Data.Repositories
{
    public class CursoRepository(AppDbContext dbContext) : Repository<Curso>(dbContext), ICursoRepository
    {
    }
}
