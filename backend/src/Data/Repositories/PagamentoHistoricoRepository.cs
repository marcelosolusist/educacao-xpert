using Business.Entities;
using Business.Interfaces;
using Data.Context;

namespace Data.Repositories
{
    public class PagamentoHistoricoRepository(AppDbContext dbContext) : Repository<PagamentoHistorico>(dbContext), IPagamentoHistoricoRepository
    {
    }
}
