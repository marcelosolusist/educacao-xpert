namespace Business.Entities
{
    public class Curso : Entity
    {
        public Curso()
        {
            Aulas = new List<Aula>();
            Matriculas = new List<Matricula>();
            Alunos = new List<Aluno>();
        }
        public string Titulo { get; set; } = null!;
        public string Instrutor { get; set; } = null!;
        public int Valor { get; set; } = 0; //O valor armazenado é em centavos
        public bool Ativo { get; set; } = true;

        public List<Aula> Aulas;
        public List<Matricula> Matriculas;
        public List<Aluno> Alunos;
    }
}
