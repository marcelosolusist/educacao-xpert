using EducacaoXpert.Core.DomainObjects.DTO;
using EducacaoXpert.PagamentoFaturamento.Domain.Entities;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;

public interface IPagamentoCartaoCreditoFacade
{
    Transacao RealizarPagamento(Pedido pedido, PagamentoCurso pagamento);
}