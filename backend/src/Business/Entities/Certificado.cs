namespace Business.Entities
{
    public class Certificado : Entity
    {
        public Guid MatriculaId { get; set; }
        public string Conteudo { get; set; } = null!;
        /* EF Relation */
        public Matricula? Matricula { get; set; }
    }
}
