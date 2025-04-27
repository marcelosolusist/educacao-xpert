using Business.Entities;
using Business.Interfaces;
using Data.Context;

namespace Data.Repositories
{
    public class MatriculaRepository(AppDbContext dbContext) : Repository<Matricula>(dbContext), IMatriculaRepository
    {
    }
}
