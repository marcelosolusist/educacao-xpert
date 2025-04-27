namespace Business.Entities
{
    public class Pagamento : Entity
    {
        public Pagamento()
        {
            PagamentosHistoricos = new List<PagamentoHistorico>();
        }
        public Guid MatriculaId { get; set; }
        public int Valor { get; set; } = 0; //O valor armazenado é em centavos
        /* EF Relation */
        public Matricula? Matricula { get; set; }

        public List<PagamentoHistorico> PagamentosHistoricos;

        public void AdicionarHistorico(StatusPagamento statusPagamento, int valor, string? autorizacaoAdquirenteId)
        {
            foreach (PagamentoHistorico pagamentoHistorico in PagamentosHistoricos)
            {
                if (pagamentoHistorico.Atual) pagamentoHistorico.Atual = false;
            }
            PagamentosHistoricos.Insert(0, new PagamentoHistorico() { PagamentoId = Id, Atual = true, Valor = valor, AutorizacaoAdquirenteId = autorizacaoAdquirenteId});
        }
        public PagamentoHistorico ObterHistoricoAtual()
        {
            return PagamentosHistoricos.FindLast(pagamentoHistorico => pagamentoHistorico.Atual);
        }
    }
}
