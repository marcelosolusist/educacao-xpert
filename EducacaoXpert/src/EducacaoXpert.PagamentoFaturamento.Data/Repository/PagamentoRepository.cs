using EducacaoXpert.Core.Data;
using EducacaoXpert.PagamentoFaturamento.Data.Context;
using EducacaoXpert.PagamentoFaturamento.Domain.Entities;
using EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducacaoXpert.PagamentoFaturamento.Data.Repository;

public class PagamentoRepository(PagamentoFaturamentoContext context) : IPagamentoRepository
{
    private readonly DbSet<Pagamento> _dbSet = context.Set<Pagamento>();
    public IUnitOfWork UnitOfWork => context;
    public void Adicionar(Pagamento pagamento)
    {
        _dbSet.Add(pagamento);
    }

    public void AdicionarTransacao(Transacao transacao)
    {
        context.Set<Transacao>().Add(transacao);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}