using EducacaoXpert.PagamentoFaturamento.Anticorruption.Interfaces;
using EducacaoXpert.PagamentoFaturamento.Domain.Entities;
using EducacaoXpert.PagamentoFaturamento.Domain.Enums;

namespace EducacaoXpert.PagamentoFaturamento.Anticorruption;

public class PayPalGateway : IPayPalGateway
{
    public string GetPayPalServiceKey(string apiKey, string encriptionKey)
    {
        return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }

    public string GetCardHashKey(string serviceKey, string cartaoCredito)
    {
        return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }

    public Transacao CommitTransaction(string cardHashKey, string orderId, int amount)
    {
        return new Transacao
        {
            MatriculaId = Guid.Parse(orderId),
            Total = amount,
            StatusTransacao = StatusTransacao.Autorizado //Sempre autorizado
        };
    }
}
