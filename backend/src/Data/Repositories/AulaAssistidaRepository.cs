using Business.Entities;
using Business.Interfaces;
using Data.Context;

namespace Data.Repositories
{
    public class AulaAssistidaRepository(AppDbContext dbContext) : Repository<AulaAssistida>(dbContext), IAulaAssistidaRepository
    {
    }
}
