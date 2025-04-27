using Business.Entities;
using Business.Interfaces;
using Data.Context;

namespace Data.Repositories
{
    public class AulaRepository(AppDbContext dbContext) : Repository<Aula>(dbContext), IAulaRepository
    {
    }
}
