using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.PagamentoFaturamento.Domain.Enums;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Entities;

public class Transacao : Entity
{
    public Guid MatriculaId { get; set; }
    public Guid PagamentoId { get; set; }
    public decimal Total { get; set; }
    public StatusTransacao StatusTransacao { get; set; }

    public Pagamento Pagamento { get; set; }
}
