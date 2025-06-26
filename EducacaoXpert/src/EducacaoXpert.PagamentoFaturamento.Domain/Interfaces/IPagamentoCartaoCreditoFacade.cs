using EducacaoXpert.PagamentoFaturamento.Domain.Entities;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;

public interface IPagamentoCartaoCreditoFacade
{
    Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento);
}