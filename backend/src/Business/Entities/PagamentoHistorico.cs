namespace Business.Entities
{
    public class PagamentoHistorico : Entity
    {
        public Guid PagamentoId { get; set; }
        public Guid StatusPagamentoId { get; set; }
        public int Valor { get; set; } = 0; //O valor armazenado é em centavos
        public string? AutorizacaoAdquirenteId { get; set; }
        public bool Atual { get; set; } = true!;
        /* EF Relation */
        public Pagamento? Pagamento { get; set; }
        public StatusPagamento? StatusPagamento { get; set; }

    }
}
