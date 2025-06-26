﻿using EducacaoXpert.PagamentoFaturamento.Anticorruption.Interfaces;
using EducacaoXpert.PagamentoFaturamento.Domain.Entities;
using EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace EducacaoXpert.PagamentoFaturamento.Anticorruption;

public class PagamentoCartaoCreditoFacade(IPayPalGateway payPalGateway,
    IOptions<PagamentoSettings> options) : IPagamentoCartaoCreditoFacade
{
    private readonly PagamentoSettings _settings = options.Value;
    public Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento)
    {
        var apiKey = _settings.ApiKey;
        var encriptionKey = _settings.EncriptionKey;

        var serviceKey = payPalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
        var cardHashKey = payPalGateway.GetCardHashKey(serviceKey, pagamento.NumeroCartao);

        var transacao = payPalGateway.CommitTransaction(cardHashKey, pedido.CursoId.ToString(), pagamento.Valor);

        transacao.PagamentoId = pagamento.Id;

        return transacao;
    }
}
