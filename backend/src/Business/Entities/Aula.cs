namespace Business.Entities
{
    public class Aula : Entity
    {
        public Aula()
        {
            AulasAssistidas = new List<AulaAssistida>();
        }
        public Guid CursoId { get; set; }
        public string Titulo { get; set; } = null!;
        public string Conteudo { get; set; } = null!;
        public int Ordem { get; set; } = 0;
        public bool Ativo { get; set; } = true;
        /* EF Relation */
        public Curso? Curso { get; set; }
        public List<AulaAssistida> AulasAssistidas;
    }
}
