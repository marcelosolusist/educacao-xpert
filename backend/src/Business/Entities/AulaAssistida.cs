namespace Business.Entities
{
    public class AulaAssistida : Entity
    {
        public Guid MatriculaId { get; set; }
        public Guid AulaId { get; set; }
        public DateTime? DataConclusao { get; set; }
        public bool Concluida { get; set; } = false;
        public bool Atual { get; set; } = false;
        /* EF Relation */
        public Matricula? Matricula { get; set; }
        public Aula? Aula { get; set; }
    }
}
