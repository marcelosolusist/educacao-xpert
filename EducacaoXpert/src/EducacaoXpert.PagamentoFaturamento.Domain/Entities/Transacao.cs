using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.PagamentoFaturamento.Domain.Enums;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Entities;

public class Transacao : Entity
{
    public Guid MatriculaId { get; set; }
    public Guid PagamentoId { get; set; }
    public int Total { get; set; } //O valor é em centavos
    public StatusTransacao StatusTransacao { get; set; }

    public Pagamento Pagamento { get; set; }
}
