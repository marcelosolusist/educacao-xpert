namespace EducacaoXpert.GestaoAlunos.Application.Queries.DTO;

public class CertificadoDto
{
    public Guid Id { get; set; }
    public string NomeAluno { get; set; }
    public string NomeCurso { get; set; }
    public DateTime DataEmissao { get; set; }
}
