namespace Business.Entities
{
    public class Matricula : Entity
    {
        public Matricula()
        {
            AulasAssistidas = new List<AulaAssistida>();
            Pagamentos = new List<Pagamento>();
            Certificados = new List<Certificado>();
    }
        public Guid CursoId { get; set; }
        public string AlunoId { get; set; } = null!;
        public int Valor { get; set; } = 0; //O valor armazenado é em centavos
        public bool Ativo { get; set; } = false;
        public bool Pago { get; set; } = false;
        public bool Concluido { get; set; } = false;
        /* EF Relation */
        public Curso? Curso { get; set; }
        public Aluno? Aluno { get; set; }

        public List<AulaAssistida> AulasAssistidas;
        public List<Pagamento> Pagamentos;
        public List<Certificado> Certificados;

        public void AdicionarOuMarcarAulaAssistidaComoAtual(Guid aulaId)
        {
            bool aulaAssistidaNovamente = false;
            foreach (AulaAssistida aulaAssistida in AulasAssistidas)
            {
                if (aulaAssistida.Atual) aulaAssistida.Atual = false;
                if (aulaAssistida.Id == aulaId)
                {
                    aulaAssistidaNovamente = true;
                    aulaAssistida.Atual = true;
                }
            }
            if (!aulaAssistidaNovamente) AulasAssistidas.Insert(0, new AulaAssistida() { MatriculaId = Id, AulaId = aulaId, Atual = true});
        }
        public AulaAssistida ObterAulaAssistidaAtual()
        {
            return AulasAssistidas.FindLast(aulaAssistida => aulaAssistida.Atual);
        }

    }
}
