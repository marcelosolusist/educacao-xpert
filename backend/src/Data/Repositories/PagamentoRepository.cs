using Business.Entities;
using Business.Interfaces;
using Data.Context;

namespace Data.Repositories
{
    public class PagamentoRepository(AppDbContext dbContext) : Repository<Pagamento>(dbContext), IPagamentoRepository
    {
    }
}
