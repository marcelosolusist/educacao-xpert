using EducacaoXpert.Core.DomainObjects.DTO;
using EducacaoXpert.PagamentoFaturamento.Anticorruption.Config;
using EducacaoXpert.PagamentoFaturamento.Anticorruption.Interfaces;
using EducacaoXpert.PagamentoFaturamento.Domain.Entities;
using EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace EducacaoXpert.PagamentoFaturamento.Anticorruption;

public class PagamentoCartaoCreditoFacade(IPayPalGateway payPalGateway,
    IOptions<PagamentoSettings> options) : IPagamentoCartaoCreditoFacade
{
    private readonly PagamentoSettings _settings = options.Value;
    public Transacao EfetuarPagamento(Pedido pedido, PagamentoCurso pagamento)
    {
        var apiKey = _settings.ApiKey;
        var encriptionKey = _settings.EncriptionKey;

        var serviceKey = payPalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
        var cardHashKey = payPalGateway.GetCardHashKey(serviceKey, pagamento.NumeroCartao);

        var transacao = payPalGateway.CommitTransaction(cardHashKey, pedido.CursoId.ToString(), pagamento.Valor);

        transacao.PagamentoId = (Guid)pagamento.PagamentoId;

        return transacao;
    }
}
