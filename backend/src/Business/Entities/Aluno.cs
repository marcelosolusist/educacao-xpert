namespace Business.Entities
{
    public class Aluno : Entity
    {
        public Aluno()
        {
            Matriculas = new List<Matricula>();
        }
        public new string Id { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public bool Ativo { get; set; } = true;

        public List<Matricula> Matriculas;
    }
}
