using EducacaoXpert.Core.DomainObjects;

namespace EducacaoXpert.GestaoAlunos.Domain.Entities;

public class Certificado : Entity
{
    public string NomeAluno { get; private set; }
    public string NomeCurso { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public string Descricao { get; private set; }
    public byte[] Arquivo { get; private set; }
    public Guid AlunoId { get; private set; }

    // EF Relations
    public Aluno Aluno { get; set; }

    // Ef Constructor
    protected Certificado() { }
    public Certificado(string nomeAluno, string nomeCurso, Guid alunoId)
    {
        NomeAluno = nomeAluno;
        NomeCurso = nomeCurso;
        AlunoId = alunoId;
        DataEmissao = DateTime.Now;
        GerarDescricao();
        Validar();
    }

    private void GerarDescricao()
    {
        Descricao = $"{NomeAluno} concluiu o curso {NomeCurso} no dia {DataEmissao:dd/MM/yyyy}.";
    }

    public void IncluirArquivo(byte[] arquivoPdf)
    {
        if (arquivoPdf == null || arquivoPdf.Length == 0)
            throw new DomainException("Arquivo do certificado inválido.");

        Arquivo = arquivoPdf;
    }

    public void Validar()
    {
        if (AlunoId == Guid.Empty)
            throw new DomainException("O campo AlunoId é obrigatório.");
        if (string.IsNullOrWhiteSpace(Descricao))
            throw new DomainException("O campo Descrição é obrigatório.");
        if (string.IsNullOrWhiteSpace(NomeAluno))
            throw new DomainException("O campo Nome Aluno é obrigatório.");
        if (string.IsNullOrWhiteSpace(NomeCurso))
            throw new DomainException("O campo Nome Curso é obrigatório.");
    }
}