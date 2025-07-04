using EducacaoXpert.Core.Data;
using EducacaoXpert.PagamentoFaturamento.Domain.Entities;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;

public interface IPagamentoRepository : IRepository<Pagamento>
{
    void Incluir(Pagamento pagamento);
    void IncluirTransacao(Transacao transacao);
}
