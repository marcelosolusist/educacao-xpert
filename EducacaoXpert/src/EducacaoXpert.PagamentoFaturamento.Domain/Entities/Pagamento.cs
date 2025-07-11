﻿using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Interfaces;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Entities;

public class Pagamento : Entity, IAggregateRoot
{
    public Guid CursoId { get; set; }
    public Guid AlunoId { get; set; }
    public int Valor { get; set; } //O valor é em centavos

    public string NomeCartao { get; set; }
    public string NumeroCartaoMascarado { get; set; }
    public Transacao Transacao { get; set; }
}
